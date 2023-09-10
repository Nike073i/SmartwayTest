using SmartwayTest.Application.Interfaces;
using SmartwayTest.Application.Models;
using SmartwayTest.Application.Options;
using SmartwayTest.DAL.Interfaces;
using SmartwayTest.DAL.Models;

namespace SmartwayTest.Application.Implementations
{
    public class FileManager : IFileManager
    {
        private readonly IFileService _fileService;
        private readonly IFileInfoDAL _fileDal;
        private readonly FileStorageOptions _fileStorageOptions;
        public event EventHandler<UploadStatusEventArgs>? UploadFileStatusChanged;

        public FileManager(
            IFileService fileService,
            IFileInfoDAL fileDal,
            FileStorageOptions fileStorageOptions
        )
        {
            _fileService = fileService;
            _fileDal = fileDal;
            _fileStorageOptions = fileStorageOptions;
        }

        public async Task<DownloadFileData> DownloadFileAsync(int fileInfoId)
        {
            var fileInfoModel =
                await _fileDal.GetByIdAsync(fileInfoId)
                ?? throw new FileNotFoundException("The file with the specified id was not found");
            return _fileService.GetFileData(fileInfoModel.FilePath, fileInfoModel.FileName);
        }

        public async Task<IEnumerable<FileViewModel>> GetAllAsync()
        {
            var models = await _fileDal.GetFilesAsync();
            return models.Select(
                model =>
                    new FileViewModel
                    {
                        Id =
                            model.FileInfoId
                            ?? throw new InvalidOperationException("Received file data without ID"),
                        Name = model.FileName
                    }
            );
        }

        public async Task SaveFileAsync(UploadFileData uploadFileData)
        {
            string fileName = uploadFileData.FileName;
            InvokeUploadFileStatusChanged(fileName, UploadFileStatus.Verification);
            if (!IsValidFile(uploadFileData))
            {
                InvokeUploadFileStatusChanged(fileName, UploadFileStatus.Error);
                throw new InvalidDataException($"File {fileName} is invalid");
            }
            string newFileName = _fileService.GenerateFileName(fileName);
            string directoryPath = _fileService.GenerateFilePath(newFileName);
            InvokeUploadFileStatusChanged(fileName, UploadFileStatus.Loading);
            try
            {
                string fullFilePath = await _fileService.UploadFileAsync(
                    uploadFileData.FileStream,
                    directoryPath,
                    newFileName
                );
                await _fileDal.AddAsync(
                    new FileInfoModel { FileName = fileName, FilePath = fullFilePath, }
                );
                InvokeUploadFileStatusChanged(fileName, UploadFileStatus.Success);
            }
            catch
            {
                InvokeUploadFileStatusChanged(fileName, UploadFileStatus.Error);
                throw;
            }
        }

        private bool IsValidFile(UploadFileData file) =>
            file.Length > 0 && file.Length < _fileStorageOptions.MaxFileSizeBytes;

        private void InvokeUploadFileStatusChanged(string fileName, UploadFileStatus newStatus)
        {
            UploadFileStatusChanged?.Invoke(
                this,
                new UploadStatusEventArgs { FileName = fileName, Status = newStatus }
            );
        }
    }
}
