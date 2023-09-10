using SmartwayTest.DAL.Models;

namespace SmartwayTest.DAL.Interfaces
{
    public interface IFileInfoDAL
    {
        Task AddAsync(FileInfoModel model);
        Task<IEnumerable<FileInfoModel>> GetFilesAsync();
        Task<FileInfoModel?> GetByIdAsync(int fileInfoId);
    }
}
