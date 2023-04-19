using WebApiBestBuy.Domain.Interfaces;
using WebApiBestBuy.Domain.Notifications;
using WebApiBestBuy.Infra.Repositories;
using WebApiBestBuy.Infra.Data;
using WebApiBestBuy.Domain.Services;
using Microsoft.AspNetCore.Identity;
using WebApiBestBuy.Infra;
using Microsoft.EntityFrameworkCore;

namespace WebApiBestBuy.Api.ExtensionServices
{
    public static class ExtensionDI
    {
        public static IServiceCollection ConfigureServices(IServiceCollection Services, IConfiguration configuration)
        {
            Services.Configure<DatabaseConfig>(configuration.GetSection("ConnectionStrings"));

            Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(configuration["ConnectionStrings:ConnectionStringEscrita"]));
            Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
            Services.AddScoped<INotificationContext, NotificationContext>();
            Services.AddScoped<IProductRepository, ProductRepository>();
            Services.AddScoped<ICartRepository, CartRepository>();
            Services.AddScoped<ICategorieRepository, CategorieRepository>();
            Services.AddScoped<ICouponRepository, CouponRepository>();

            return Services;
        }

    }
}
