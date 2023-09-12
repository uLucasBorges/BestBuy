using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using WebApiBestBuy.Domain.Interfaces;
using WebApiBestBuy.Domain.Interfaces.Repositories;
using WebApiBestBuy.Domain.Interfaces.Services;
using WebApiBestBuy.Domain.Notifications;
using WebApiBestBuy.Domain.ViewModel;
namespace WebApiBestBuy.Api.Controllers
{
    [Route("[Controller]")]
    [Authorize]
    public class CartController : BaseController
    {
         private readonly ICartService _cartService;

        public CartController(ICartService cartService, INotificationContext notificationContext) : base (notificationContext)
        {
            _cartService = cartService;
        }

        
        [HttpGet("Products/Remove")]
        public async Task<IActionResult> DeleteProductsInCart(int productId, int quantity)
        {
            var cartId = base.CreateCartId();

            await _cartService.RemoveProductCart(productId, quantity, cartId);

            return Response();
        }


        [HttpPost("Products/Add")]
        public async Task<IActionResult> IncluirCarrinho(int ProductId, int Quantity)
        {
            var cartId = CreateCartId();

            await _cartService.InsertOrUpdate(cartId, ProductId, Quantity);

            return Response();
        }


        [HttpGet("List/Products")]
        public async Task<IActionResult> GetProductsInCart()
        {
            var cartId = base.CreateCartId();

            var products = await _cartService.GetProductsByCart(cartId);

            return Response(products);
        }
    }
}
