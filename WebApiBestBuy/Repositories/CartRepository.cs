using System.Data;
using BestBuy.Core.Interfaces;
using WebApiBestBuy.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using WebApiBestBuy.ViewModel;

namespace BestBuy.Infra.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly IConfiguration _config;
        private readonly ICouponRepository _couponRepository;
        private readonly string stringConexao;


        public CartRepository(IConfiguration config, ICouponRepository couponRepository)
        {
            stringConexao = config.GetSection("ConnectionStrings").GetSection("Default").Value;
            _couponRepository = couponRepository;
        }

        public async Task<bool> AddCoupon(string cartId, string couponCode)
        {
            var cartExists = await this.ExistCart(cartId);

            if (cartExists)
            {
                var coupon = await _couponRepository.ExistsCouponOrCartHaveCoupon(couponCode, null);
                if (coupon.Coupon.CouponCode == "")
                await _couponRepository.ApplyCoupon(cartId, couponCode);

                return true;
            }

            return false;


        }

        public async Task<bool> AddProductCart(string CartId, int ProductId, int AmountInsert)
        {

                using (var connection = new SqlConnection(stringConexao))
                {
                connection.Open();

                var query = "exec InsertOrUpdateCart \r\n@cartId = @CartId,\r\n@productId = @ProductId,\r\n@amountInsert = @AmountInsert";
                    
                 connection.Execute(query, new {
                     cartId = CartId,
                     productId = ProductId,
                     amountInsert = AmountInsert
                 });

                connection.Dispose();

                return true;
                }
            }


        public async Task<bool> ExistCart(string cartID)
        {
            using (var connection = new SqlConnection(stringConexao))
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
            using (var connection = new SqlConnection(stringConexao))
            {
                connection.Open();

                var query = "SELECT C.Id, P.Name , C.Quantity , C.ValueTotal \r\n  FROM CART C \r\n  INNER JOIN PRODUCTS P ON P.Id = C.ProductId \r\n  WHERE C.ID = @CartId";

                IEnumerable<Product> productsInCart = await connection.QueryAsync<Product>(query, new { cartId = CartId });

                var existsCoupon = await _couponRepository.ExistsCouponOrCartHaveCoupon("", CartId);
               
                connection.Dispose();

                var controle = 0.0;

                if (existsCoupon != null)
                {
                    foreach(Product obj in productsInCart)
                    {
                        controle += obj.ValueTotal;
                    }

                    controle -= existsCoupon.Coupon.DiscountAmount;
                }


                return new CartVM
                {
                    Products = productsInCart,
                    DiscountAmount = existsCoupon.Coupon.DiscountAmount,
                    Price = controle 
                };

                
            }
        }

        public async Task<bool> RemoveProductCart(int ProductId, string CartID)
        {
            using (var connection = new SqlConnection(stringConexao))
            {
                var query = "delete from cart where Id = @CartID";
                var exec = await connection.ExecuteAsync(query, new { Id = CartID });

                return true;

            }
        }

    }
}
