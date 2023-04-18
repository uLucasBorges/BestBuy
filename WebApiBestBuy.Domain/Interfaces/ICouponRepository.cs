using WebApiBestBuy.Domain.Models;
using WebApiBestBuy.Domain.ViewModel;

namespace WebApiBestBuy.Domain.Interfaces;

    public interface ICouponRepository
    {
        public Task<Coupon> CreateCoupon(string couponCode, double Amount);
        public Task<bool> DeleteCoupon(string couponCode);
        public Task<ResultViewModel> ExistsCoupon(string couponCode );
        public Task<ResultViewModel> CartHaveCoupon(string CartId);
        public Task<bool> ApplyCoupon(string cartId, string couponCode);
        public Task<bool> RemoveCoupon(string cartId);


    }

