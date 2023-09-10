using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using SmartwayTest.Application.Implementations;
using SmartwayTest.Application.Interfaces;

namespace SmartwayTest.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services
                .AddSingleton<IContentTypeProvider, FileExtensionContentTypeProvider>()
                .AddSingleton<IFileService, FileService>()
                .AddSingleton<IDownloadTokenService, DownloadTokenService>()
                .AddSingleton<IFileManager, FileManager>();
            return services;
        }
    }
}
