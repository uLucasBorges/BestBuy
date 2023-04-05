using BestBuy.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BestBuy.Core.Notification;

namespace WebApiBestBuy.Controllers
{
    public class CartController : BaseController
    {
        private readonly ICartRepository cartRepository;
        private readonly INotificationContext notificationContext;


        public CartController(ICartRepository cartRepository, INotificationContext notificationContext) : base(notificationContext)
        {
            this.cartRepository = cartRepository;
        }

        [HttpDelete("cart/Remove-Cart")]
        public async Task<IActionResult> DeleteProductsInCart(int productId)
        {
            var cartId = base.CreateCartId();

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
            var cartId = base.CreateCartId();

            var exists = await cartRepository.ExistCart(cartId);

            if (!exists)
                return BadRequest();

            var products = await cartRepository.GetProductsByCart(cartId);

            return Ok(products);
        }
    }
}
