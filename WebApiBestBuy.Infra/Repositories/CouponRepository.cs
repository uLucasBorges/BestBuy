using WebApiBestBuy.Domain.Interfaces;
using WebApiBestBuy.Domain.Notifications;
using WebApiBestBuy.Infra.Data;
using WebApiBestBuy.Domain.Models;
using WebApiBestBuy.Domain.ViewModel;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace WebApiBestBuy.Infra.Repositories;

    public class CouponRepository : ICouponRepository
    {
    private string   ConnectionStringEscrita { get; }
    private readonly INotificationContext _notificationContext;

        public CouponRepository(IOptions<DatabaseConfig> config, INotificationContext notificationContext)
        {
            ConnectionStringEscrita = config.Value.Clearsale_Write;
            _notificationContext = notificationContext;
        }

        public async Task<bool> ApplyCoupon(string CartId, string CouponCode)
        {
            using (var connection =  new SqlConnection(ConnectionStringEscrita))
            {
                if (!string.IsNullOrEmpty(CartId) && !string.IsNullOrWhiteSpace(CouponCode))
                {
                    var coupon = await ExistsCoupon(CouponCode);

                    if (!coupon.Success)
                    {
                        return false;
                    }

                    var query = "INSERT CartHeaders (UserId,CouponCode) values (@cartId, @couponCode)";
                    await connection.ExecuteAsync(query, new { cartId = CartId, couponCode = coupon.data.CouponCode });

                connection.Dispose();


                _notificationContext.AddNotification(200, "Cupom Aplicado");
                    return true;
                }

                return false;
            }
        }

        public async Task<Coupon> CreateCoupon(string couponCode, double discountAmount)
        {
            using (var connection =  new SqlConnection(ConnectionStringEscrita))
            {
                var query = "INSERT COUPON (COUPONCODE,DiscountAmount) VALUES (@couponCode,@discountAmount)";

                await connection.ExecuteAsync(query, new { CouponCode = couponCode, DiscountAmount = discountAmount });

                connection.Dispose();

            return new Coupon { DiscountAmount = discountAmount, CouponCode = couponCode};
            }
        }

        public async Task<bool> DeleteCoupon(string couponCode)
        {
            using (var connection =  new SqlConnection(ConnectionStringEscrita))
            {
                var query = "DELETE FROM Coupon WHERE CouponCode = @couponCode";

                await connection.ExecuteAsync(query, new { CouponCode = couponCode });
                connection.Dispose();

            return true;

            }
        }

        public async Task<ResultViewModel> ExistsCoupon(string? couponCode)
        {
            using (var connection =  new SqlConnection(ConnectionStringEscrita))
            {
                connection.Open();
                var query = "SELECT CouponCode, DiscountAmount FROM Coupon WHERE CouponCode = @couponCode";

          
                var result = (await connection.QueryAsync<Coupon>(query, new { CouponCode = couponCode})).FirstOrDefault();
                connection.Dispose();

            if (result != null)
                {
                    return new ResultViewModel { data = result, Success = true };
                }

                _notificationContext.AddNotification(404, "Ticket inexistente");

                return new ResultViewModel { Success = false };

            }
        }  
        
        
        public async Task<ResultViewModel> CartHaveCoupon(string userid)
        {
            using (var connection =  new SqlConnection(ConnectionStringEscrita))
            {
                connection.Open();
                
                 var query = "\r\nSELECT ch.CouponCode , c.DiscountAmount from CartHeaders ch inner join Coupon c on c.CouponCode = ch.CouponCode\r\n and ch.UserId = @userid";

                var result = (await connection.QueryAsync<Coupon>(query, new {UserId = userid}))?.FirstOrDefault();
                connection.Dispose();

            if (result != null)
                {
                    _notificationContext.AddNotification(200, "Carrinho já possui Cupom");

                    return new ResultViewModel { data = result, Success = true };
                }

                return new ResultViewModel { Success = false, data = new Coupon() };

            }
        }

        public async Task<bool> RemoveCoupon(string userid)
        {
            if (String.IsNullOrEmpty(userid))
                return false;

            using (var connection =  new SqlConnection(ConnectionStringEscrita))
            {
              connection.Dispose();

            var query = "delete from CartHeaders  where UserId = @userid";
                var result = await connection.ExecuteAsync(query, new
                {
                    UserId = userid
                });

                return true;
            }
        }
    }

