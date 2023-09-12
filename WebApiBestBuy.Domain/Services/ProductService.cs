
using WebApiBestBuy.Domain.Interfaces.Repositories;
using WebApiBestBuy.Domain.Interfaces.Services;
using WebApiBestBuy.Domain.Models;
using WebApiBestBuy.Domain.Notifications;
using WebApiBestBuy.Domain.ViewModel;

namespace WebApiBestBuy.Domain.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly INotificationContext _notificationContext;

        public ProductService(IProductRepository productRepository, INotificationContext notificationContext)
        {
            _productRepository = productRepository;
            _notificationContext = notificationContext;
        }

        public async Task<Product> CreateProduct(Product product)
        {
            if (product.IsValid)
            {
                await _productRepository.CreateProduct(product);
            }

            _notificationContext.AddNotification(400, product.Erros);

            return product;
        }

        public async Task DeleteProduct(int id)
        {
             await _productRepository.GetProduct(id);

            if (_notificationContext.HasNotifications())
                return;

           await _productRepository.DeleteProduct(id);
        

        }

        public async Task<ResultViewModel> GetProduct(int Id) =>  await _productRepository.GetProduct(Id);
        

        public async Task<ResultViewModel> GetProducts() => await _productRepository.GetProducts();
            
        

        public async Task UpdateProduct(Product product)
        {
            if (!product.IsValid)
                return;

            var productExists = await GetProduct(product.Id);

            if (_notificationContext.HasNotifications())
                return;

            await _productRepository.UpdateProduct(productExists.Data, product);
        }
    }
}
