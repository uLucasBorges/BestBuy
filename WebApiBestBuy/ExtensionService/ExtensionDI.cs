using WebApiBestBuy.Domain.Interfaces;
using WebApiBestBuy.Domain.Notifications;
using WebApiBestBuy.Infra.Repositories;
using WebApiBestBuy.Infra.Data;
using WebApiBestBuy.Domain.Services;
using WebApiBestBuy.Infra;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WebApiBestBuy.Domain.Models;

namespace WebApiBestBuy.Api.ExtensionServices
{
    public static class ExtensionDI
    {
        public static IServiceCollection ConfigureServices(IServiceCollection Services, IConfiguration configuration)
        {
            Services.Configure<DatabaseConfig>(configuration.GetSection("ConnectionStrings"));

            Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(configuration["ConnectionStrings:ConnectionStringEscrita"]));


            Services.AddScoped<IUserService, UserService>();
            Services.AddScoped<INotificationContext, NotificationContext>();
            Services.AddScoped<IProductRepository, ProductRepository>();
            Services.AddScoped<ICartRepository, CartRepository>();
            Services.AddScoped<ICategorieRepository, CategorieRepository>();
            Services.AddScoped<ICouponRepository, CouponRepository>();


            var autoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserAccount, IdentityUser>().ReverseMap();
               
            });

            Services.AddSingleton(autoMapperConfig.CreateMapper());

            return Services;
        }

    }
}
