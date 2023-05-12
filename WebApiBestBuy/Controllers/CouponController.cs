using Microsoft.AspNetCore.Mvc;
using WebApiBestBuy.Domain.Interfaces;
using WebApiBestBuy.Domain.Models;
using WebApiBestBuy.Domain.Notifications;
using WebApiBestBuy.Domain.ViewModel;

namespace WebApiBestBuy.Api.Controllers
{

    [Route("[Controller]")]
    public class CouponController : BaseController
    {
        private readonly ICouponRepository couponRepository;
        private readonly INotificationContext _notificationContext;

        public CouponController(ICouponRepository couponRepository, INotificationContext _notificationContext) : base (_notificationContext)
        {
            this.couponRepository = couponRepository;
        }

        [HttpPost("/Create")]
        public async Task<IActionResult> CreateCoupon([FromBody] Coupon coupon)
        {
            if (!coupon.IsValid)
            {
                return Response(coupon.Erros);
            }

            ResultViewModel? existsCoupon = await couponRepository.ExistsCoupon(coupon.CouponCode);


            if (!existsCoupon.Success)
            {
                await couponRepository.CreateCoupon(coupon);

                return Ok(new ResultViewModel { Data = coupon, Success = true, Message = "Cupom criado com Sucesso!"});
            }

            return BadRequest("Cupon já existente");
        }

        [HttpDelete("/Delete")]
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

        [HttpPost("/Apply")]
        public async Task<IActionResult> ApplyCoupon(string couponCode) {

            var cartId = base.CreateCartId();

            if (!string.IsNullOrEmpty(couponCode) && !string.IsNullOrEmpty(cartId))
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

        [HttpDelete("/Remove")]
        public async Task<IActionResult> RemoveCoupon()
        {
            var result = await couponRepository.RemoveCoupon(base.CreateCartId());
            if (result)
                return Ok("Cupom removido com sucesso");

            return NotFound("O carrinho não possui cupons");
        }
    }
}
