using WebApiBestBuy.Models;
using WebApiBestBuy.ViewModel;

namespace WebApiBestBuy.Domain.Interfaces
{
    public interface ICategorieRepository
    {
        Task<Categorie> CreateCategory(Categorie categorie);
        Task<ResultViewModel> GetCategorie(int Id);

        Task<bool> DeleteCategory(int CategorieId);
    }
}
