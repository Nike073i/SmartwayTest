namespace SmartwayTest.Application.Models
{
    public class UploadStatusEventArgs : EventArgs
    {
        public UploadFileStatus Status { get; set; }
        public string FileName { get; set; } = null!;
    }
}
