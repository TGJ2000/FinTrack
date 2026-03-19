using Microsoft.Data.SqlClient;

namespace FinTrack.Repositories
{
    public interface IDbConnectionFactory
    {
        SqlConnection CreateConnection();
    }
}
