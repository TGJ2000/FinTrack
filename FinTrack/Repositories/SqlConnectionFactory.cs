using Microsoft.Data.SqlClient;

namespace FinTrack.Repositories
{
    public class SqlConnectionFactory(string connectionString) : IDbConnectionFactory
    {
        public SqlConnection CreateConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
