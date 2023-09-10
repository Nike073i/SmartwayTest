using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.StaticFiles;
using SmartwayTest.Application.Interfaces;
using SmartwayTest.Application.Models;
using SmartwayTest.Application.Options;

namespace SmartwayTest.Application.Implementations
{
    public class FileService : IFileService
    {
        private readonly FileStorageOptions _fileStorageOptions;
        private readonly IContentTypeProvider _contentTypeProvider;

        public FileService(
            IContentTypeProvider contentTypeProvider,
            FileStorageOptions fileStorageOptions
        )
        {
            _fileStorageOptions = fileStorageOptions;
            _contentTypeProvider = contentTypeProvider;
        }

        public string GenerateFileName(string fileName)
        {
            string fileExtension = Path.GetExtension(fileName);
            string uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            return uniqueFileName;
        }

        public string GenerateFilePath(string fileName)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(fileName);
            byte[] hashBytes = MD5.HashData(inputBytes);
            string hash = Convert.ToHexString(hashBytes);
            return Path.Combine(
                    Directory.GetCurrentDirectory(),
                    _fileStorageOptions.BaseFileDirectory,
                    hash.AsSpan(0, 2).ToString(),
                    hash.AsSpan(0, 4).ToString()
                )
                .ToString();
        }

        public async Task<string> UploadFileAsync(
            Stream fileFormStream,
            string directory,
            string fileName
        )
        {
            CreateFolder(directory);
            string filePath = Path.Combine(directory, fileName);
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await fileFormStream.CopyToAsync(fileStream);
            return filePath;
        }

        public DownloadFileData GetFileData(string filePath, string fileName)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File on path {filePath} doesn't exist.");

            _contentTypeProvider.TryGetContentType(filePath, out string? mimeType);
            return new DownloadFileData
            {
                FullPath = filePath,
                Name = fileName,
                MimeType = mimeType ?? MediaTypeNames.Application.Octet
            };
        }

        private static void CreateFolder(string dir)
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }
    }
}
