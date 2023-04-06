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
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            if (product.IsValid)
            {
                var result = await _productRepository.CreateProduct(product);
                if (result != null)
                    return StatusCode(201);
            }

            return BadRequest(product.Erros);
        }
        
        [HttpGet("Products")]
        public async Task<IActionResult> GetProducts()
        {
            var aluno = await _productRepository.GetProduct(1);
            var alunos = await _productRepository.GetProducts();

            return Ok();
        }
        
        [HttpPut("Products")]
        public async Task<IActionResult> UpdateProducts(Product product)
        {
          await _productRepository.UpdateProduct(product);

            return Ok();
        }
    }
}
