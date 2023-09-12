using Microsoft.IdentityModel.Tokens;
using WebApiBestBuy.Domain.Interfaces.Repositories;
using WebApiBestBuy.Domain.Interfaces.Services;
using WebApiBestBuy.Domain.Models;

namespace WebApiBestBuy.Domain.Services
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepository _couponRepository;

        public CouponService(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }

        public async Task ApplyCoupon(string cartId, string couponCode)
        {
            var haveCoupon = await _couponRepository.CartHaveCoupon(cartId);

            if (haveCoupon)
                return;

            var existsCupom = await _couponRepository.ExistsCoupon(couponCode);
            if (existsCupom)
            {
             await  _couponRepository.ApplyCoupon(cartId, couponCode);
            }
            
        }

        public async Task CreateCoupon(Coupon coupon)
        {
           if (!coupon.IsValid)
                return;


           var existsCupom = await _couponRepository.ExistsCoupon(coupon.CouponCode);
               
           if (existsCupom)
           await _couponRepository.CreateCoupon(coupon);
  
        }

        public async Task DeleteCoupon(string couponCode)
        {
            var existsCupom = await _couponRepository.ExistsCoupon(couponCode);
            
            if (existsCupom)
                await _couponRepository.DeleteCoupon(couponCode);
        }

        public async Task RemoveCoupon(string cartId)
        {
            var cartHaveCoupon = await _couponRepository.CartHaveCoupon(cartId);

            if (cartHaveCoupon)
             await  _couponRepository.RemoveCoupon(cartId);

        }

    }
}
