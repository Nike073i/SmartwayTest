using SmartwayTest.Application.Options;
using SmartwayTest.FileWebService.Consts;

namespace SmartwayTest.FileWebService.Configs
{
    public class FileStorageConfig
    {
        public static FileStorageOptions GetFileStorageOptions(IConfiguration configuration)
        {
            var fileStorageSection = configuration.GetRequiredSection(
                ConfigurationKeys.FileStorageKey
            );
            var fileStorageOptions = new FileStorageOptions();
            fileStorageSection.Bind(fileStorageOptions);
            return fileStorageOptions;
        }
    }
}
