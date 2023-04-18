using WebApiBestBuy.Domain.Interfaces;
using WebApiBestBuy.Domain.Notifications;
using WebApiBestBuy.Infra.Repositories;
using WebApiBestBuy.Infra.Data;
using WebApiBestBuy.Domain.Services;

namespace WebApiBestBuy.Api.ExtensionServices
{
    public static class ExtensionDI
    {
        public static IServiceCollection ConfigureServices(IServiceCollection Services)
        {
           Services.AddScoped<AppDbContext>();
           Services.AddScoped<INotificationContext, NotificationContext>();
           Services.AddScoped<IProductRepository, ProductRepository>();
           Services.AddScoped<ICartRepository, CartRepository>();
           Services.AddScoped<ICategorieRepository, CategorieRepository>();
           Services.AddScoped<ICouponRepository, CouponRepository>();
           Services.AddScoped<IUserService, UserService>();


            return Services;
        }

    }
}
