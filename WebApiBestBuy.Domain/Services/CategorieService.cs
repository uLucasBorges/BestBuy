


using WebApiBestBuy.Domain.Interfaces.Repositories;
using WebApiBestBuy.Domain.Interfaces.Services;
using WebApiBestBuy.Domain.Models;
using WebApiBestBuy.Domain.Notifications;

namespace WebApiBestBuy.Domain.Services
{
    public class CategorieService : ICategorieService
    {
        private readonly ICategorieRepository _categorieRepository;
        private readonly INotificationContext _notificationContext;

        public CategorieService(ICategorieRepository categorieRepository, INotificationContext notificationContext)
        {
           _categorieRepository = categorieRepository;
            _notificationContext = notificationContext;
        }

      

        public async Task CreateCategory(Categorie categorie)
        {
            if (categorie.IsValid)
            {
                var exists = await _categorieRepository.GetCategorie(categorie.Name);

                if (exists.Success)
                {
                    _notificationContext.AddNotification(400, "Categoria já existente");
                    return;
                }

               await _categorieRepository.CreateCategory(categorie);
            }
        }


        public async Task<bool> DeleteCategory(int id)
        {
            if(id != 0)
            {
                await _categorieRepository.DeleteCategory(id);
                return true;
            }

            return false;
        }
    }
}
