using SmartwayTest.DAL.Models;

namespace SmartwayTest.DAL.Interfaces
{
    public interface IDownloadTokenDAL
    {
        Task<Guid> CreateAsync(int fileInfoId);
        Task<DownloadTokenModel?> GetLockAsync(Guid downloadTokenId);
        Task MakeAsUsedAsync(Guid downloadTokenId);
    }
}
