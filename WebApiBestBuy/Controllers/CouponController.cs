using BestBuy.Core.Interfaces;
using BestBuy.Core.Notification;
using Microsoft.AspNetCore.Mvc;

namespace WebApiBestBuy.Controllers
{
    [Route("[Controller]")]
    public class CouponController : BaseController
    {
        private readonly ICouponRepository couponRepository;
        private readonly INotificationContext _notificationContext;
        public CouponController(ICouponRepository couponRepository, INotificationContext notificationContext) : base(notificationContext)
        {
            this.couponRepository = couponRepository;
        }

        [HttpPost("/CreateCoupon")]
        public async Task<IActionResult> CreateCoupon(string couponCode, double amount)
        {
            var existsCoupon = await couponRepository.ExistsCoupon(couponCode);

            if (!existsCoupon.Success)
            {
                var couponCreated = await couponRepository.CreateCoupon(couponCode, amount);

                return Ok(couponCreated);
            }

            return BadRequest("Cupon já existente");
        }

        [HttpDelete("/DeleteCoupon")]
        public async Task<IActionResult> DeleteCoupon(string code) 
        {

            var coupon = await couponRepository.ExistsCoupon(code);

            if(coupon.Success)
            {
               await couponRepository.DeleteCoupon(code);
                return Ok("Cupom deletado");
            }

            return NotFound("Cupom não existente");
        
        }

        [HttpPost("/ApplyCoupon")]
        public async Task<IActionResult> ApplyCoupon(string couponCode) {

            var cartId = Request.Cookies["CartId"];

        if(!string.IsNullOrEmpty(couponCode) && !string.IsNullOrEmpty(cartId))
            {

                var couponExists = await couponRepository.ExistsCoupon(couponCode);

                if (couponExists.Success)
                {
                    var cartHaveCoupon = await couponRepository.CartHaveCoupon(cartId);

                    if (!cartHaveCoupon.Success )
                    {
                        await couponRepository.ApplyCoupon(cartId, couponCode);
                    }

                }
            }

            return Response();
        }


    }
}
