using BestBuy.Core.Interfaces;
using BestBuy.Core.Notification;
using Dapper;
using Microsoft.Data.SqlClient;
using WebApiBestBuy.Data;
using WebApiBestBuy.Models;
using WebApiBestBuy.ViewModel;

namespace BestBuy.Infra.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly AppDbContext _context;
        private readonly INotificationContext _notificationContext;

        public CouponRepository(AppDbContext context, INotificationContext notificationContext)
        {
            _context = context;
            _notificationContext = notificationContext;
        }

        public async Task<bool> ApplyCoupon(string CartId, string CouponCode)
        {
            using (var connection = _context.Connect())
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

                    _notificationContext.AddNotification(200, "Cupom Aplicado");
                    return true;
                }

                return false;
            }
        }

        public async Task<Coupon> CreateCoupon(string couponCode, double discountAmount)
        {
            using (var connection = _context.Connect())
            {
                var query = "INSERT COUPON (COUPONCODE,DiscountAmount) VALUES (@couponCode,@discountAmount)";

                await connection.ExecuteAsync(query, new { CouponCode = couponCode, DiscountAmount = discountAmount });

                return new Coupon { DiscountAmount = discountAmount, CouponCode = couponCode};
            }
        }

        public async Task<bool> DeleteCoupon(string couponCode)
        {
            using (var connection = _context.Connect())
            {
                var query = "DELETE FROM Coupon WHERE CouponCode = @couponCode";

                await connection.ExecuteAsync(query, new { CouponCode = couponCode });
                return true;

            }
        }

        public async Task<ResultViewModel> ExistsCoupon(string? couponCode)
        {
            using (var connection = _context.Connect())
            {
                connection.Open();
                var query = "SELECT CouponCode, DiscountAmount FROM Coupon WHERE CouponCode = @couponCode";

          
                var result = (await connection.QueryAsync<Coupon>(query, new { CouponCode = couponCode})).FirstOrDefault();
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
            using (var connection = _context.Connect())
            {
                connection.Open();
                
                 var query = "\r\nSELECT ch.CouponCode , c.DiscountAmount from CartHeaders ch inner join Coupon c on c.CouponCode = ch.CouponCode\r\n and ch.UserId = @userid";

                var result = (await connection.QueryAsync<Coupon>(query, new {UserId = userid}))?.FirstOrDefault();
               
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

            using (var connection = _context.Connect())
            {
                var query = "delete from CartHeaders  where UserId = @userid";
                var result = await connection.ExecuteAsync(query, new
                {
                    UserId = userid
                });

                return true;
            }
        }
    }
}
