using Microsoft.AspNetCore.Mvc;
using WebApiBestBuy.Domain.Interfaces;
using WebApiBestBuy.Domain.Notifications;
using WebApiBestBuy.Domain.ViewModel;
using WebApiBestBuy.Infra.Repositories;

namespace WebApiBestBuy.Api.Controllers
{
    [Route("[Controller]")]
    public class CartController : BaseController
    {
        private readonly ICartRepository cartRepository;
        private readonly INotificationContext notificationContext;
        private readonly IUnitOfWork _unitOfWork;

        public CartController(ICartRepository cartRepository, INotificationContext notificationContext, IUnitOfWork unitOfWork) : base(notificationContext)
        {
            this.cartRepository = cartRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpDelete("Remove/Product")]
        public async Task<IActionResult> DeleteProductsInCart(int productId, int quantityAmount)
        {
            var cartId = base.CreateCartId();

            var exists = await cartRepository.ExistCart(cartId);

            if(exists)
            await cartRepository.RemoveProductCart(productId, quantityAmount, cartId);

            return Response();
        }


        [HttpPost("Add/Product")]
        public async Task<IActionResult> IncluirCarrinho(int ProductId, int Quantity)
        {
            var cartId = CreateCartId();

            await cartRepository.InsertOrUpdate(cartId, ProductId, Quantity);

            return Response();
        }


        [HttpGet("List/Products")]
        public async Task<IActionResult> GetProductsInCaty()
        {
            var cartId = base.CreateCartId();

            var exists = await _unitOfWork.CartRepository.ExistCart(cartId);

            if (!exists) 
                return NotFound(new ResultViewModel { Success = false, Data = "O Carrinho está vazio!" });

            var products = await cartRepository.GetProductsByCart(cartId);
        
            return Ok(products);
        }
    }
}
