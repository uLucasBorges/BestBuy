using System.Data;
using WebApiBestBuy.Domain.Interfaces;
using WebApiBestBuy.Domain.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using WebApiBestBuy.Domain.ViewModel;
using WebApiBestBuy.Domain.Notifications;
using WebApiBestBuy.Infra.Data;
using Microsoft.Extensions.Options;

namespace WebApiBestBuy.Infra.Repositories
{
    public class CartRepository : ICartRepository
    {
        private string   ConnectionStringEscrita { get; }
        private readonly ICouponRepository _couponRepository;
        private readonly INotificationContext _notificationContext;


        public CartRepository(IOptions<DatabaseConfig> config ,ICouponRepository couponRepository, INotificationContext notificationContext)
        {
            ConnectionStringEscrita = config.Value.Clearsale_Write;
            _notificationContext = notificationContext;
            _couponRepository = couponRepository;
        }

        public async Task<bool> AddCoupon(string cartId, string couponCode)
        {
            var cartExists = await this.ExistCart(cartId);

            if (cartExists)
            {
                var coupon = await _couponRepository.ExistsCoupon(couponCode);

                if (coupon.Success)
                {
                    await _couponRepository.ApplyCoupon(cartId, couponCode);

                    return true;
                }
            }

            return false;


        }

        public async Task<bool> AddProductCart(string CartId, int ProductId, int AmountInsert)
        {

            using (var connection =  new SqlConnection(ConnectionStringEscrita))
            {
                connection.Open();

                var cart = await ExistCart(CartId);

                if (!cart)
                {
                    _notificationContext.AddNotification(404, "Carrinho não encontrado");
                    return false;
                }

                connection.Open();

                var query = "exec InsertOrUpdateCart \r\n@cartId = @CartId,\r\n@productId = @ProductId,\r\n@amountInsert = @AmountInsert";

                var result = connection.Execute(query, new
                {
                    cartId = CartId,
                    productId = ProductId,
                    amountInsert = AmountInsert
                });

                connection.Dispose();

                if (result == -1) { 
                _notificationContext.AddNotification(404, "Produto não encontrado");
                return false;
                }

                return true;
                }  
            }


        public async Task<bool> ExistCart(string cartID)
        {
            using (var connection =  new SqlConnection(ConnectionStringEscrita))
            {
                connection.Open();

                var query = "select * from cart where Id = @CartID";
                var exec = await connection.QueryAsync<Cart>(query, new { CartID = cartID });
               
                connection.Dispose();

                if (exec != null)
                    return true;

                   return false;

            }
        }

        public async Task<CartVM> GetProductsByCart(string CartId)
        {
            using (var connection =  new SqlConnection(ConnectionStringEscrita))
            {
                connection.Open();

                var query = "SELECT C.Id, P.Name , C.Quantity , C.ValueTotal \r\n  FROM CART C \r\n  INNER JOIN PRODUCTS P ON P.Id = C.ProductId \r\n  WHERE C.ID = @CartId";

                IEnumerable<ProductViewModel> productsInCart = await connection.QueryAsync<ProductViewModel>(query, new { cartId = CartId });

                var existsCoupon = await _couponRepository.CartHaveCoupon(CartId);
               
                connection.Dispose();

                var controle = 0.0;

                if (existsCoupon.Success)
                {
                    foreach(ProductViewModel obj in productsInCart)
                    {
                        controle += obj.ValueTotal;
                    }

                    controle -= existsCoupon.data.DiscountAmount;

                    if (controle < 0)
                        controle = 0;
                }


                return new CartVM
                {
                    Products = productsInCart,
                    DiscountAmount = existsCoupon.data.DiscountAmount,
                    Price = controle 
                };

                
            }
        }

        public async Task<bool> RemoveProductCart(int ProductId, int AmountInsert, string CartID)
        {
            using (var connection =  new SqlConnection(ConnectionStringEscrita))
            {
                connection.Open();
                var cart = await ExistCart(CartID);
                if (cart)
                {
                    var query = "exec RemoveOrDeleteCart \r\n@cartId = @CartId,\r\n@productId = @ProductId,\r\n@amountInsert = @AmountInsert";

                    var result = connection.Execute(query, new
                    {
                        cartId = CartID,
                        productId = ProductId,
                        amountInsert = AmountInsert
                    });

                    connection.Dispose();

                    if (result == -1)
                    {
                        _notificationContext.AddNotification(404, "Produto não encontrado");
                        return false;
                    }

                    return true;
                }

                _notificationContext.AddNotification(404, "Carrinho inexistente");
                return false;

            }
        }

    }
}
