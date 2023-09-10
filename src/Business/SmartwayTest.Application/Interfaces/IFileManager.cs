using SmartwayTest.Application.Models;

namespace SmartwayTest.Application.Interfaces
{
    public interface IFileManager
    {
        event EventHandler<UploadStatusEventArgs>? UploadFileStatusChanged;
        Task SaveFileAsync(UploadFileData uploadFileData);
        Task<DownloadFileData> DownloadFileAsync(int fileInfoId);
        Task<IEnumerable<FileViewModel>> GetAllAsync();
    }
}
