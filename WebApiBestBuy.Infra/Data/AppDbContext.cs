
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApiBestBuy.Infra.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext( DbContextOptions<AppDbContext> options): base (options) {}

    }
}
