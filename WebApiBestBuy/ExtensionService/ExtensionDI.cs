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
using WebApiBestBuy.Domain.Interfaces.Repositories;
using WebApiBestBuy.Domain.Interfaces.Services;
using WebApiBestBuy.DTOS;

namespace WebApiBestBuy.Api.ExtensionServices
{
    public static class ExtensionDI
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection Services, IConfiguration configuration)
        {
            Services.Configure<DatabaseConfig>(configuration.GetSection("ConnectionStrings"));
            Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(configuration["ConnectionStrings:ConnectionStringEscrita"]));
            Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
           
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddScoped<ICartService, CartService>();
            Services.AddScoped<INotificationContext, NotificationContext>();
            Services.AddScoped<IProductRepository, ProductRepository>();
            Services.AddScoped<ICartRepository, CartRepository>();
            Services.AddScoped<ICategorieRepository, CategorieRepository>();
            Services.AddScoped<ICouponRepository, CouponRepository>();
            Services.AddScoped<IUserService, UserService>();

            #region Mapper
            IMapper mapper = Config.RegisterMaps().CreateMapper();
            Services.AddSingleton(mapper);
            Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            #endregion

            var autoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserAccount, IdentityUser>().ReverseMap();
               
            });

            Services.AddSingleton(autoMapperConfig.CreateMapper());

            return Services;
        }

    }
}
