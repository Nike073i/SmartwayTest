namespace SmartwayTest.Application.Models
{
    public class UploadFileData
    {
        public Stream FileStream { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public long Length { get; set; }
    }
}
