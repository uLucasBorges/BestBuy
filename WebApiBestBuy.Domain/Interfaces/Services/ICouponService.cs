

using WebApiBestBuy.Domain.Interfaces.Repositories;
using WebApiBestBuy.Domain.Models;

namespace WebApiBestBuy.Domain.Interfaces.Services
{
    public interface ICouponService
    {
        public  Task ApplyCoupon(string cartId, string couponCode);
        public  Task CreateCoupon(Coupon coupon);

        public  Task DeleteCoupon(string couponCode);

        public Task RemoveCoupon(string cartId);


    }
}
