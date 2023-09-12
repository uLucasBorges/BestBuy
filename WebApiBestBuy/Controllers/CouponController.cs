using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiBestBuy.Domain.Interfaces.Repositories;
using WebApiBestBuy.Domain.Interfaces.Services;
using WebApiBestBuy.Domain.Models;
using WebApiBestBuy.Domain.Notifications;
using WebApiBestBuy.Domain.ViewModel;

namespace WebApiBestBuy.Api.Controllers
{

    [Route("[Controller]")]
    [Authorize]
    public class CouponController : BaseController
    {
        private readonly ICouponService _couponService;
        private readonly INotificationContext _notificationContext;

        public CouponController(ICouponService couponService, INotificationContext _notificationContext) : base (_notificationContext)
        {
            _couponService = couponService;
        }

        [HttpPost("/Create")]
        public async Task<IActionResult> CreateCoupon([FromBody] Coupon coupon)
        {
            if (!coupon.IsValid)
            {
                return Response(coupon.Erros);
            }
            await _couponService.CreateCoupon(coupon);

            return Response();
            
        }

        [HttpDelete("/Delete")]
        public async Task<IActionResult> DeleteCoupon(string code) 
        {
         await _couponService.DeleteCoupon(code);
         return Response(); 
        }

        [HttpPost("/Apply")]
        public async Task<IActionResult> ApplyCoupon(string couponCode) {

            var cartId = base.CreateCartId();


            if (!string.IsNullOrEmpty(couponCode) && !string.IsNullOrEmpty(cartId))
            {
             await _couponService.ApplyCoupon(cartId, couponCode); 
            }

            return Response();
        }

        [HttpDelete("/Remove")]
        public async Task<IActionResult> RemoveCoupon()
        {
             await _couponService.RemoveCoupon(base.CreateCartId());
            return Response();
        }
    }
}
