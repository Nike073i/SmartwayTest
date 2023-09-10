using System.Data.Common;

namespace SmartwayTest.DAL.Connections
{
    public interface IDbConnectionFabric
    {
        DbConnection CreateConnection();
    }
}
