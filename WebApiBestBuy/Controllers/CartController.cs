using BestBuy.Core.Interfaces;
using WebApiBestBuy.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApiBestBuy.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartRepository cartRepository;
        public CartController(ICartRepository cartRepository)
        {
            this.cartRepository = cartRepository;
        }

        [HttpDelete("cart/Remove-Cart")]
        public async Task<IActionResult> DeleteProductsInCart(int productId)
        {
            var cartId = CreateCartId();

            var exists = await cartRepository.ExistCart(cartId);

            if(exists)
            await cartRepository.RemoveProductCart(productId, cartId);

            return Ok();
        }


        [HttpPost("cart/Add-Product")]
        public async Task<IActionResult> IncluirCarrinho(int ProductId, int Quantity)
        {
            var cartId = CreateCartId();

            await cartRepository.AddProductCart(cartId, ProductId, Quantity);

            return Ok();
        }


        [HttpGet("cart/Get-Products")]
        public async Task<IActionResult> GetProductsInCaty()
        {
            var cartId = CreateCartId();

            var exists = await cartRepository.ExistCart(cartId);

            if (!exists)
                return BadRequest();

            var products = await cartRepository.GetProductsByCart(cartId);

            return Ok(products);
        }


        private string CreateCartId()
        {
            var cart = Request.Cookies["CartId"];

            if (string.IsNullOrEmpty(cart))
            {
                var id = Guid.NewGuid();
              

                Response.Cookies.Append("CartID", id.ToString());
                return id.ToString();
            }

            return cart;

        }
    }
}
