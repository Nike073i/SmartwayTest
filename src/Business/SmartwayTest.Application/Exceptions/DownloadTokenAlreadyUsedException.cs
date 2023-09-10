namespace SmartwayTest.Application.Exceptions
{
    public class DownloadTokenIsInvalidException : Exception
    {
        public DownloadTokenIsInvalidException(string message)
            : base(message) { }
    }
}
