﻿using Microsoft.AspNetCore.Mvc;
using WebApiBestBuy.Domain.Interfaces;
using WebApiBestBuy.Domain.Notifications;

namespace WebApiBestBuy.Api.Controllers
{
    public class CartController : BaseController
    {
        private readonly ICartRepository cartRepository;
        private readonly INotificationContext notificationContext;


        public CartController(ICartRepository cartRepository, INotificationContext notificationContext) : base(notificationContext)
        {
            this.cartRepository = cartRepository;
        }

        [HttpDelete("cart/Remove-Product")]
        public async Task<IActionResult> DeleteProductsInCart(int productId, int quantityAmount)
        {
            var cartId = base.CreateCartId();

            var exists = await cartRepository.ExistCart(cartId);

            if(exists)
            await cartRepository.RemoveProductCart(productId, quantityAmount, cartId);

            return Response();
        }


        [HttpPost("cart/Add-Product")]
        public async Task<IActionResult> IncluirCarrinho(int ProductId, int Quantity)
        {
            var cartId = CreateCartId();

            await cartRepository.AddProductCart(cartId, ProductId, Quantity);

            return Response();
        }


        [HttpGet("cart/Products")]
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
