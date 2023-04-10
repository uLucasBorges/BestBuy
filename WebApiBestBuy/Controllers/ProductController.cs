using BestBuy.Core.Interfaces;
using BestBuy.Core.Notification;
using Microsoft.AspNetCore.Mvc;
using WebApiBestBuy.Models;

namespace WebApiBestBuy.Controllers
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

        [HttpGet("/By/{Id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            await _productRepository.GetProduct(id);
            return Response();
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
            
           return Ok(Response(result));
        }
    }
}
