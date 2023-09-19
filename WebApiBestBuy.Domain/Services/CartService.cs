using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiBestBuy.Domain.Interfaces.Repositories;
using WebApiBestBuy.Domain.Interfaces.Services;
using WebApiBestBuy.Domain.Models;
using WebApiBestBuy.Domain.Notifications;
using WebApiBestBuy.Domain.ViewModel;

namespace WebApiBestBuy.Domain.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICouponRepository _couponRepository;
        private readonly IProductRepository _productRepository;
        private readonly INotificationContext _notificationContext;

        public CartService(
            ICartRepository cartRepository, 
            ICouponRepository couponRepository, 
            IProductRepository productRepository, 
            INotificationContext notificationContext)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _notificationContext = notificationContext;
            _couponRepository = couponRepository;
        }

      
        public async Task AddCoupon(string cartId, string couponCode)
        {
            await _cartRepository.ExistCart(cartId);
            if (_notificationContext.HasNotifications())
            return;
            

            await _couponRepository.ExistsCoupon(couponCode);
            if (_notificationContext.HasNotifications())
            return;
            

            await _couponRepository.ApplyCoupon(cartId, couponCode);
               
        }

        public async Task<CartVM> GetProductsByCart(string cartId)
        {
            var products = await _cartRepository.GetProductsByCart(cartId);

            if (products.Products.Any()) {
               
                var haveCoupon = await _couponRepository.CartHaveCoupon(cartId);

                if (haveCoupon)
                {
                    Coupon Cupon =  await _couponRepository.GetCouponByCart(cartId);

                    products.DiscountAmount = Cupon.DiscountAmount;
                    products.CartPriceWithDiscount = (products.Products.Sum(x => x.ValueTotal)) -  Cupon.DiscountAmount;
                    
                }
            }

            return products;

        }

        public async Task InsertOrUpdate(string CartId, int ProductId, double AmountInsert)
        {

             await _productRepository.GetProduct(ProductId);


            if (_notificationContext.HasNotifications())
            {
                return;
            }

            await _cartRepository.InsertOrUpdate(CartId, ProductId, AmountInsert);

            if (_notificationContext.HasNotifications())
            {
                return;
            }


        }

        public async Task<bool> RemoveProductCart(int productId, int quantity, string cartId)
        {
            var carTSearched = await _cartRepository.ExistCart(cartId);

            if (carTSearched) {

                var productsInCart = await _cartRepository.GetProductsByCart(cartId);

                var SearchedProductinCart = productsInCart.Products.Where(X => X.Id == productId);

                if (SearchedProductinCart.Any())
                {
                    await _cartRepository.RemoveProductCart(productId, quantity, cartId);
                    return true;
                }
                
            }

    

            return false;
               
        }



        //metodo não está sendo utilizado, pois estou tirando a dependencia da proc e crie o metoddo InsertOrUpdate com a lógica
        //public async Task<bool> AddProductCart(string CartId, int ProductId, int AmountInsert)
        //{

        //    using (var context = _context)
        //    {
        //        var cart = await ExistCart(CartId);

        //        if (!cart)
        //        {
        //            _notificationContext.AddNotification(404, "Carrinho não encontrado");
        //            return false;
        //        }

        //        //var query = "exec InsertOrUpdateCart \r\n@cartId = @CartId,\r\n@productId = @ProductId,\r\n@amountInsert = @AmountInsert";

        //        _context.Begin();

        //        var result = await context.connection.ExecuteAsync(@"InsertOrUpdateCart", new
        //        {
        //            cartId = CartId,
        //            productId = ProductId,
        //            amountInsert = AmountInsert
        //        }, transaction: _context.transaction,
        //        commandType: CommandType.StoredProcedure);

        //        _context.Commit();


        //        if (result == -1)
        //        {
        //            _notificationContext.AddNotification(404, "Produto não encontrado");
        //            return false;

        //        }

        //        return true;
        //    }
        //}
    }
}

