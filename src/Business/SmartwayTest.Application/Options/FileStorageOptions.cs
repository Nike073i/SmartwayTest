namespace SmartwayTest.Application.Options
{
    public class FileStorageOptions
    {
        public string BaseFileDirectory { get; set; } = "files";
        public int MaxFileSizeBytes { get; set; } = 2 * 1024 * 1024;
    }
}
