namespace SmartwayTest.DAL.Models
{
    public class DownloadTokenModel
    {
        public Guid? DownloadTokenId { get; set; }
        public int FileInfoId { get; set; }
        public bool IsUsed { get; set; }
    }
}
