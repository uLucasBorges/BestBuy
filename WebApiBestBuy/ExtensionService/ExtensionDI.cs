using BestBuy.Core.Interfaces;
using BestBuy.Core.Notification;
using BestBuy.Infra.Repositories;
using WebApiBestBuy.Data;
using WebApiBestBuy.Services;

namespace BestBuy.Api.ExtensiosDI
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
