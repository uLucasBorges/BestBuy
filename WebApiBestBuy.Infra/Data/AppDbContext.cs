using System.Data;
using System.Data.Common;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace WebApiBestBuy.Infra.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext( ) {}

    }
}
