
using WebApiBestBuy.Domain.Models;

namespace WebApiBestBuy.Domain.Interfaces.Services
{
    public interface ICategorieService
    {
        public  Task CreateCategory(Categorie categorie);
        public Task<bool> DeleteCategory(int id);


    }
}
