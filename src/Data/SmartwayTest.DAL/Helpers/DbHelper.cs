using Dapper;
using SmartwayTest.DAL.Connections;

namespace SmartwayTest.DAL.Helpers
{
    public class DbHelper
    {
        public static async Task ExecuteAsync(
            IDbConnectionFabric dbConnectionFabric,
            string sql,
            object? model = null
        )
        {
            using var connection = dbConnectionFabric.CreateConnection();
            await connection.OpenAsync();
            await connection.ExecuteAsync(sql, model);
        }

        public static async Task<T?> QueryScalarAsync<T>(
            IDbConnectionFabric dbConnectionFabric,
            string sql,
            object? model = null
        )
        {
            using var connection = dbConnectionFabric.CreateConnection();
            await connection.OpenAsync();
            return await connection.QueryFirstOrDefaultAsync<T>(sql, model);
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(
            IDbConnectionFabric dbConnectionFabric,
            string sql,
            object? model = null
        )
        {
            using var connection = dbConnectionFabric.CreateConnection();
            await connection.OpenAsync();
            return await connection.QueryAsync<T>(sql, model);
        }
    }
}
