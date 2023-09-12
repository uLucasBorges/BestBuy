using WebApiBestBuy.Domain.Models;
using WebApiBestBuy.Domain.ViewModel;

namespace WebApiBestBuy.Domain.Interfaces.Repositories;

public interface IProductRepository
{

    public Task<Product> CreateProduct(Product product);
    public Task<bool> DeleteProduct(int ProducId);
    public Task<ResultViewModel> GetProduct(int ProducId);
    public Task<bool> ExistsProduct(int ProducId);

    public Task<ResultViewModel> GetProducts();
    public Task<ResultViewModel> UpdateProduct(Product product, Product newProduct);

}

