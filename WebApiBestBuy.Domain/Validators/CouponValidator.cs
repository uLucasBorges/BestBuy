
using FluentValidation;
using WebApiBestBuy.Domain.Models;

namespace WebApiBestBuy.Domain.Validators
{
    public class CouponValidator : AbstractValidator<Coupon>
    {
        public CouponValidator()
        {
            RuleFor(x => x.CouponCode)
                .NotEmpty()
                .WithMessage("O Codigo do cupom não deve ser vazio");

            RuleFor(x => x.DiscountAmount)
                .NotEmpty()
                .WithMessage("O Desconto do cupom não deve ser vazio");
        }
    }
}
