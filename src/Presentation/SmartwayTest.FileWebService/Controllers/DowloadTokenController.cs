using Microsoft.AspNetCore.Mvc;
using SmartwayTest.Application.Exceptions;
using SmartwayTest.Application.Interfaces;

namespace SmartwayTest.FileWebService.Controllers
{
    /// <summary>
    /// Сontroller for download tokens
    /// </summary>
    public class DowloadTokenController : BaseApiController
    {
        private readonly IDownloadTokenService _downloadTokenService;

        public DowloadTokenController(IDownloadTokenService downloadTokenService)
        {
            _downloadTokenService = downloadTokenService;
        }

        /// <summary>
        /// Generate token for file
        /// </summary>
        /// <param name="fileInfoId">File ID</param>
        /// <returns>Token for download</returns>
        /// <response code="200">The token was generated successfully</response>
        [HttpPost("{fileInfoId:int}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult> GenerateDownloadToken(int fileInfoId)
        {
            try
            {
                return Ok(await _downloadTokenService.GenerateDownloadTokenAsync(fileInfoId));
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "An error occurred while creating the download token. Please try again later"
                );
            }
        }

        /// <summary>
        /// Download file by token
        /// </summary>
        /// <param name="downloadToken">Token</param>
        /// <returns>File</returns>
        /// <response code="200">The token was successfully applied</response>
        /// <response code="400">The token is invalid</response>
        [HttpGet("{downloadToken:guid}")]
        [ProducesResponseType(typeof(PhysicalFileResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DownloadFileByToken(Guid downloadToken)
        {
            try
            {
                var downloadData = await _downloadTokenService.DownloadFileAsync(downloadToken);

                return PhysicalFile(
                    contentType: downloadData.MimeType,
                    physicalPath: downloadData.FullPath,
                    fileDownloadName: downloadData.Name
                );
            }
            catch (DownloadTokenIsInvalidException invalidEx)
            {
                return BadRequest(invalidEx.Message);
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "An error occurred while downloading file. Please try again later"
                );
            }
        }
    }
}
