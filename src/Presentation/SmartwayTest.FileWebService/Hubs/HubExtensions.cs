namespace SmartwayTest.FileWebService.Hubs
{
    public static class HubExtensions
    {
        public static void MapHubs(this WebApplication app) =>
            app.MapHub<FileUploadHub>("/hubs/fileUpload");
    }
}
