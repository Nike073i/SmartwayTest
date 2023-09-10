using Microsoft.Extensions.DependencyInjection;
using SmartwayTest.DAL.Connections;
using SmartwayTest.DAL.Interfaces;
using SmartwayTest.DAL.PostgreSQL.Connections;
using SmartwayTest.DAL.PostgreSQL.Implementations;

namespace SmartwayTest.DAL.PostgreSQL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPostgresDb(
            this IServiceCollection services,
            string dbConnectionString
        )
        {
            services.AddSingleton<IDbConnectionFabric, PostgresDbConnectionFabric>(
                provider => new PostgresDbConnectionFabric(dbConnectionString)
            );
            services
                .AddSingleton<IFileInfoDAL, FileInfoDAL>()
                .AddSingleton<IDownloadTokenDAL, DownloadTokenDAL>();
            return services;
        }
    }
}
