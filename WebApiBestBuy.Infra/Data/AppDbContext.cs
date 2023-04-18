using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace WebApiBestBuy.Infra.Data
{
    public class AppDbContext
    {
        private IDbConnection dbConnection;
        private readonly IConfiguration configuration;

        public AppDbContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IDbConnection Connect()
        {
            var connectionString = configuration.GetConnectionString("Default");
            dbConnection = new SqlConnection(connectionString);

            return dbConnection;
        }
    }
}
