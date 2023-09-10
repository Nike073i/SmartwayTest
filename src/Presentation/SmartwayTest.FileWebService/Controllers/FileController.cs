using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SmartwayTest.Application.Interfaces;
using SmartwayTest.Application.Models;
using SmartwayTest.FileWebService.Hubs;

namespace SmartwayTest.FileWebService.Controllers
{
    /// <summary>
    /// File operations controller
    /// </summary>
    public class FileController : BaseApiController
    {
        private readonly IFileManager _fileManager;
        private string? _clientConnectionId;
        private readonly IHubContext<FileUploadHub>? _hubContext;

        public FileController(IFileManager fileManager, IHubContext<FileUploadHub> hubContext)
        {
            _fileManager = fileManager;
            _hubContext = hubContext;
        }

        /// <summary>
        /// Uploading files to the server
        /// </summary>
        /// <param name="files">Files for upload</param>
        /// <param name="connectionId">ID of the client connection to the hub</param>
        /// <response code="200">All files were uploaded successfully</response>
        /// <response code="400">One or more uploaded files are invalid</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UploadFile(
            [FromForm] IFormFile[] files,
            [FromForm] string? connectionId
        )
        {
            if (files.Any())
                BadRequest("Invalid file data");
            if (!string.IsNullOrEmpty(connectionId))
            {
                _clientConnectionId = connectionId;
                _fileManager.UploadFileStatusChanged += SendFileUploadStatus;
            }
            try
            {
                foreach (var file in files)
                {
                    using var fileStream = file.OpenReadStream();
                    await _fileManager.SaveFileAsync(
                        new UploadFileData
                        {
                            FileName = file.FileName,
                            FileStream = fileStream,
                            Length = file.Length
                        }
                    );
                }
            }
            catch (InvalidDataException ide)
            {
                return BadRequest(ide.Message);
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "An error occurred while uploading files. Please try again later"
                );
            }
            finally
            {
                _fileManager.UploadFileStatusChanged -= SendFileUploadStatus;
            }
            return Ok();
        }

        /// <summary>
        /// Download the file from the server
        /// </summary>
        /// <param name="fileInfoId">File ID</param>
        /// <returns>File</returns>
        /// <response code="200">The file was downloaded successfully</response>
        /// <response code="400">The file with the specified ID does not exist</response>
        [HttpGet("{fileInfoId:int}")]
        [ProducesResponseType(typeof(PhysicalFileResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DownloadFile(int fileInfoId)
        {
            try
            {
                var downloadData = await _fileManager.DownloadFileAsync(fileInfoId);
                return PhysicalFile(
                    contentType: downloadData.MimeType,
                    physicalPath: downloadData.FullPath,
                    fileDownloadName: downloadData.Name
                );
            }
            catch (FileNotFoundException fnfe)
            {
                return BadRequest(fnfe.Message);
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "An error occurred while downloading file. Please try again later"
                );
            }
        }

        /// <summary>
        /// Provide information about stored files
        /// </summary>
        /// <returns>File data</returns>
        /// <response code="200">Returned list of stored files</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FileViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllFile()
        {
            try
            {
                return Ok(await _fileManager.GetAllAsync());
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "An error occurred while getting files. Please try again later"
                );
            }
        }

        private void SendFileUploadStatus(object? sender, UploadStatusEventArgs args) =>
            _hubContext?.Clients.Client(_clientConnectionId!).SendAsync("UpdateStatus", args);
    }
}
