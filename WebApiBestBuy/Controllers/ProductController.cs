using WebApiBestBuy.Domain.Notifications;
using Microsoft.AspNetCore.Mvc;
using WebApiBestBuy.Domain.Models;
using WebApiBestBuy.Domain.Interfaces;

namespace WebApiBestBuy.Api.Controllers
{
    [Route("[Controller]")]
    public class ProductController : BaseController
    {
        IProductRepository _productRepository;
        INotificationContext _notificationContext;

        public ProductController(IProductRepository productRepository, INotificationContext notificationContext) : base (notificationContext)
        {
            _productRepository = productRepository;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            if (product.IsValid)
            {
                 await _productRepository.CreateProduct(product); 
            }
            return Response();
        }
        
        [HttpGet("List")]
        public async Task<IActionResult> GetProducts() 
        {  
           var products =  await _productRepository.GetProducts();
            return Response(products);
        }

        [HttpGet("/By/{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var result = await _productRepository.GetProduct(id);
            return Response(result);
        }


        [HttpPut("Update")]
        public async Task<IActionResult> UpdateProducts(Product product)
        {
            if (product.IsValid)
            {
               var result = await _productRepository.UpdateProduct(product);
                return Response(result);

            }
             
            return BadRequest(product.Erros);
            
        }


        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteProducts(int id)
        {
           var result = await _productRepository.DeleteProduct(id);
            
           return Response();
        }
    }
}
