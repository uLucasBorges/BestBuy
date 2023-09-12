using WebApiBestBuy.Domain.Models;
using WebApiBestBuy.Domain.ViewModel;

namespace WebApiBestBuy.Domain.Interfaces.Repositories;

public interface ICategorieRepository
{
    Task CreateCategory(Categorie categorie);
    Task<ResultViewModel> GetCategorie(string CategoryName);

    Task DeleteCategory(int CategorieId);
}
