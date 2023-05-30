using AutoMapper;
using WebApiBestBuy.Domain.Models;
using WebApiBestBuy.Domain.ViewModel;

namespace WebApiBestBuy.DTOS
{
    public class Config
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CategorieViewModel, Categorie>().ReverseMap();
              
            });

            return mappingConfig;
        }

    }
}
