using SmartwayTest.Application.Exceptions;
using SmartwayTest.Application.General;
using SmartwayTest.Application.Interfaces;
using SmartwayTest.Application.Models;
using SmartwayTest.DAL.Interfaces;

namespace SmartwayTest.Application.Implementations
{
    public class DownloadTokenService : IDownloadTokenService
    {
        private readonly IDownloadTokenDAL _downloadTokenDal;
        private readonly IFileInfoDAL _fileInfoDal;
        private readonly IFileService _fileService;

        public DownloadTokenService(
            IDownloadTokenDAL downloadTokenDal,
            IFileInfoDAL fileInfoDal,
            IFileService fileService
        )
        {
            _downloadTokenDal = downloadTokenDal;
            _fileService = fileService;
            _fileInfoDal = fileInfoDal;
        }

        public async Task<DownloadFileData> DownloadFileAsync(Guid downloadTokenId)
        {
            using var scope = Helpers.CreateTransactionScope();
            var downloadTokenModel =
                await _downloadTokenDal.GetLockAsync(downloadTokenId)
                ?? throw new DownloadTokenIsInvalidException("Token does not exist");
            if (downloadTokenModel.IsUsed)
                throw new DownloadTokenIsInvalidException("Token already used");
            var fileInfoModel =
                await _fileInfoDal.GetByIdAsync(downloadTokenModel.FileInfoId)
                ?? throw new FileNotFoundException(
                    $"The file with the specified id - {downloadTokenModel.FileInfoId}  was not found"
                );
            await _downloadTokenDal.MakeAsUsedAsync(downloadTokenId);
            scope.Complete();
            return _fileService.GetFileData(fileInfoModel.FilePath, fileInfoModel.FileName);
        }

        public async Task<Guid> GenerateDownloadTokenAsync(int fileInfoId) =>
            await _downloadTokenDal.CreateAsync(fileInfoId);
    }
}
