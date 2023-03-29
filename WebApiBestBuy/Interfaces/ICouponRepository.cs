using WebApiBestBuy.Models;

namespace BestBuy.Core.Interfaces
{
    public interface ICouponRepository
    {
        public Task<Coupon> CreateCoupon(string couponCode, double Amount);
        public Task<bool> DeleteCoupon(string couponCode);
        public Task<ResultVM> ExistsCoupon(string couponCode );
        public Task<ResultVM> CartHaveCoupon(string CartId);
        public Task<bool> ApplyCoupon(string cartId, string couponCode);

    }
}
