
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
                .NotEqual(0.0)
                .WithMessage("O Desconto do cupom deve ser maior que $0,00")

                .NotEmpty()
                .WithMessage("O Codigo do cupom não deve ser vazio");
        }
    }
}
