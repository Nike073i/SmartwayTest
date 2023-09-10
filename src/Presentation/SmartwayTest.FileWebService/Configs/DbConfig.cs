using SmartwayTest.FileWebService.Consts;

namespace SmartwayTest.FileWebService.Configs
{
    public static class DbConfig
    {
        public static string GetConnectionString(IConfiguration configuration)
        {
            string dbConnectionString =
                configuration[ConfigurationKeys.DatabaseConnectionKey]
                ?? throw new InvalidOperationException("Database connection string not set");
            return dbConnectionString;
        }
    }
}
