
using FluentValidation;
using WebApiBestBuy.Models;

namespace WebApiBestBuy.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("O nome do produto não pode ser vazio.")

                .NotNull()
                .WithMessage("o nome do produto não deve ser nulo.")
            
                .MinimumLength(5)
                .WithMessage("O nome do produto deve ter no mínimo 5 caracteres.")

                .MaximumLength(50)
                .WithMessage("O nome do produto ter no máximo 50 caracteres.");

        }
    }
}
