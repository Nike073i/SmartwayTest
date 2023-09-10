using Npgsql;
using SmartwayTest.DAL.Connections;
using System.Data.Common;

namespace SmartwayTest.DAL.PostgreSQL.Connections
{
    public class PostgresDbConnectionFabric : IDbConnectionFabric
    {
        private readonly string _connectionString;

        public PostgresDbConnectionFabric(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
    }
}
