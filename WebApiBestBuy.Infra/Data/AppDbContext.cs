
using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApiBestBuy.Domain.Interfaces;

namespace WebApiBestBuy.Infra.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {

        private readonly DatabaseConfig _dbConfig;
        public AppDbContext(IOptions<DatabaseConfig> config, DbContextOptions<AppDbContext> options): base (options) {
            _dbConfig = config.Value;
        }


        public IDbConnection? connection { get; private set; }
        public IDbTransaction? transaction { get; private set; }
        Guid _id = Guid.Empty;


        public void Begin()
        {
            if (connection?.State == ConnectionState.Closed) {
                connection.Open();
                transaction = connection.BeginTransaction();
                return;

            }

            if (connection == null)
            {
                connection = new SqlConnection(_dbConfig.ConnectionStringEscrita);
                connection.Open();
                transaction = connection.BeginTransaction();
                return;
            }

        }

        public void Commit()
        {
            transaction?.Commit();

            connection?.Close();
            Dispose();
        }

        public void Rollback()
        {

            transaction?.Rollback();

            connection?.Close();

            Dispose();
        }

        public override void Dispose()
        {
            transaction?.Dispose();
            connection?.Close();
        }

    }
}
