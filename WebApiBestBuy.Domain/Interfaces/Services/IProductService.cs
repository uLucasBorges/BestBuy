
using WebApiBestBuy.Domain.Interfaces.Repositories;
using WebApiBestBuy.Domain.Models;
using WebApiBestBuy.Domain.ViewModel;

namespace WebApiBestBuy.Domain.Interfaces.Services
{
    public interface IProductService
    {
        public  Task<Product> CreateProduct(Product product);
        public Task DeleteProduct(int id);
        public  Task<ResultViewModel> GetProduct(int Id);
        public  Task<ResultViewModel> GetProducts();
        public Task UpdateProduct(Product product);
        
    }
}
