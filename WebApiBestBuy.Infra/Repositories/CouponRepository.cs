using WebApiBestBuy.Domain.Notifications;
using WebApiBestBuy.Infra.Data;
using WebApiBestBuy.Domain.Models;
using WebApiBestBuy.Domain.ViewModel;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using WebApiBestBuy.Domain.Interfaces.Repositories;

namespace WebApiBestBuy.Infra.Repositories;

public class CouponRepository : ICouponRepository
    {
    private string   ConnectionStringEscrita { get; }
    private readonly INotificationContext _notificationContext;

        public CouponRepository(IOptions<DatabaseConfig> config, INotificationContext notificationContext)
        {
            ConnectionStringEscrita = config.Value.ConnectionStringEscrita;
            _notificationContext = notificationContext;
        }

        public async Task<bool> ApplyCoupon(string CartId, string CouponCode)
        {
            using (var connection =  new SqlConnection(ConnectionStringEscrita))
            {
                    var query = "INSERT CartHeaders (UserId,CouponCode) values (@cartId, @couponCode)";
                    await connection.ExecuteAsync(query, new { cartId = CartId, couponCode = CouponCode });

                    connection.Dispose();


               return true;
            }
        }

        public async Task<Coupon> CreateCoupon(Coupon coupon)
        {
            using (var connection =  new SqlConnection(ConnectionStringEscrita))
            {
                var query = "INSERT COUPON (COUPONCODE,DiscountAmount) VALUES (@couponCode,@discountAmount)";

                await connection.ExecuteAsync(query, new { CouponCode = coupon.CouponCode, DiscountAmount = coupon.DiscountAmount });

                connection.Dispose();

            return coupon;
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

        public async Task<bool> ExistsCoupon(string? couponCode)
        {
            using (var connection =  new SqlConnection(ConnectionStringEscrita))
            {
                connection.Open();
                var query = "IF EXISTS (SELECT TOP 1 1 FROM Coupon WHERE CouponCode = @couponCode) SELECT 1 ELSE SELECT 0";

          
                var result = (await connection.QueryAsync<bool>(query, new { CouponCode = couponCode})).FirstOrDefault();
                connection.Dispose();

            if (result)
                return true; 

            _notificationContext.AddNotification(404, "Cupom inexistente");
                return false;

            }
        }  
        
        
        public async Task<bool> CartHaveCoupon(string userid)
        {
            using (var connection =  new SqlConnection(ConnectionStringEscrita))
            {
                connection.Open();
                
                 var query = @"IF EXISTS(SELECT ch.CouponCode , c.DiscountAmount 
                              from CartHeaders ch 
                              inner join Coupon c on c.CouponCode = ch.CouponCode 
                              and ch.UserId = @userid) SELECT 1 ELSE SELECT 0";

                var result = (await connection.QueryAsync<bool>(query, new {UserId = userid})).FirstOrDefault();
               
               connection.Dispose();
               
               if (result)
               return true;


               return false;
 
        }
    }

        public async Task<bool> RemoveCoupon(string userid)
        {
         
            using (var connection =  new SqlConnection(ConnectionStringEscrita))
            {

            var query = "delete from CartHeaders  where UserId = @userid";
               
            
            var result = await connection.ExecuteAsync(query, new
                {
                    UserId = userid
                });

            connection.Dispose();

            return true;
            }
        }

    public async Task<Coupon> GetCouponByCart(string UserId)
    {
       using(var connection = new SqlConnection(ConnectionStringEscrita))
        {
            connection.Open();

            var query = @"SELECT
                          C.CouponCode,
                          C.DiscountAmount
                          FROM [clearsale].[dbo].[CartHeaders] CH
                          INNER JOIN COUPON C ON C.COUPONCODE = CH.COUPONCODE
                          WHERE [UserId] = @UserId";

            var searchedCoupon = (await connection.QueryAsync<Coupon>(query, new { UserId = UserId })).FirstOrDefault();
            
            return searchedCoupon;
        }
    }
}

