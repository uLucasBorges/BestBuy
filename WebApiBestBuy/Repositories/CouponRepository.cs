using BestBuy.Core.Interfaces;
using BestBuy.Core.Notification;
using Dapper;
using Microsoft.Data.SqlClient;
using WebApiBestBuy.Models;

namespace BestBuy.Infra.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly INotificationContext _notificationContext;
        private readonly IConfiguration _config;
        private readonly string stringConexao;

        public CouponRepository(IConfiguration config, INotificationContext notificationContext)
        {
            _config = config;
            _notificationContext = notificationContext;
            stringConexao = _config.GetSection("ConnectionStrings").GetSection("Default").Value;
        }

  

        public async Task<bool> ApplyCoupon(string CartId, string CouponCode)
        {
            using (var connection = new SqlConnection(stringConexao))
            {
                if (!string.IsNullOrEmpty(CartId) && !string.IsNullOrWhiteSpace(CouponCode))
                {
                    var query = "INSERT CartHeaders (UserId,CouponCode) values (@cartId, @couponCode)";
                    await connection.ExecuteAsync(query, new { cartId = CartId, couponCode = CouponCode });

                    return true;
                }

                return false;
            }
        }

        public async Task<Coupon> CreateCoupon(string couponCode, double discountAmount)
        {
            using(var connection = new SqlConnection(stringConexao))
            {
                var query = "INSERT COUPON (COUPONCODE,DiscountAmount) VALUES (@couponCode,@discountAmount)";

                await connection.ExecuteAsync(query, new { CouponCode = couponCode, DiscountAmount = discountAmount });

                return new Coupon { DiscountAmount = discountAmount, CouponCode = couponCode};
            }
        }

        public async Task<bool> DeleteCoupon(string couponCode)
        {
            using (var connection = new SqlConnection(stringConexao))
            {
                var query = "DELETE FROM Coupon WHERE CouponCode = @couponCode";

                await connection.ExecuteAsync(query, new { CouponCode = couponCode });
                return true;

            }
        }

        public async Task<ResultVM> ExistsCoupon(string? couponCode)
        {
            using(var connection = new SqlConnection(stringConexao))
            {
                connection.Open();
                var query = "SELECT CouponCode, DiscountAmount FROM Coupon WHERE CouponCode = @couponCode";

          
                var result = (await connection.QueryAsync<Coupon>(query, new { CouponCode = couponCode})).FirstOrDefault();
                if (result != null)
                {
                    _notificationContext.AddNotification(200, "Cupom Encontrado");

                    return new ResultVM { Coupon = result, Success = true };
                }

                _notificationContext.AddNotification(404, "cupom inexistente");
                return new ResultVM { Success = false };

            }
        }  
        
        
        public async Task<ResultVM> CartHaveCoupon(string userid)
        {
            using(var connection = new SqlConnection(stringConexao))
            {
                connection.Open();
                
                 var query = "\r\nSELECT ch.CouponCode , c.DiscountAmount from CartHeaders ch inner join Coupon c on c.CouponCode = ch.CouponCode\r\n and ch.UserId = @userid";

                var result = (await connection.QueryAsync<Coupon>(query, new {UserId = userid})).FirstOrDefault();
               
                if (result != null)
                {
                    _notificationContext.AddNotification(200, "Carrinho já possui Cupom");

                    return new ResultVM { Coupon = result, Success = true };
                }

                _notificationContext.AddNotification(404, "Não há cupom");
                return new ResultVM { Success = false };

            }
        }
    }
}
