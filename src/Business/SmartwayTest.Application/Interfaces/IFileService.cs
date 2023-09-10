using SmartwayTest.Application.Models;

namespace SmartwayTest.Application.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(Stream fileFormStream, string directory, string fileName);
        string GenerateFileName(string fileName);
        string GenerateFilePath(string fileName);
        DownloadFileData GetFileData(string filePath, string fileName);
    }
}
