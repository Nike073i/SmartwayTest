using SmartwayTest.Application.Models;

namespace SmartwayTest.Application.Interfaces
{
    public interface IDownloadTokenService
    {
        public Task<Guid> GenerateDownloadTokenAsync(int fileInfoId);
        public Task<DownloadFileData> DownloadFileAsync(Guid downloadTokenId);
    }
}
