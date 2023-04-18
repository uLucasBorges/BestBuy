
using FluentValidation;
using WebApiBestBuy.Domain.Models;

namespace WebApiBestBuy.Domain.Validators
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

            RuleFor(x => x.CategoryId)
                .NotEqual(0)
                .WithMessage("A Categoria do produto não pode ser 0")

                .NotEmpty()
                .WithMessage("Você deve possuir uma categoria")

                .NotNull()
                .WithMessage("A categoria não pode ser nula");

            RuleFor(x => x.Price)
            .NotEqual(0)
            .WithMessage("O Valor produto não pode ser $0,00")

            .NotEmpty()
            .WithMessage("O Valor do produto não pode ser vazio.")

            .NotNull()
            .WithMessage("O Valor do produto não deve ser nulo.");

            RuleFor(X => X.Description)
                .NotEmpty()
                .WithMessage("A Descrição do produto não pode ser vazia.")

                .NotNull()
                .WithMessage("A Descrição do produto não deve ser nula.")

                .MinimumLength(5)
                .WithMessage("A Descrição do produto deve ter no mínimo 5 caracteres.")

                .MaximumLength(50)
                .WithMessage("A Descrição do produto ter no máximo 50 caracteres.");


            RuleFor(x => x.ImageUrl)
                .NotEmpty()
                .WithMessage("A URL do produto não pode ser vazia.")

                .NotNull()
                .WithMessage("A URL do produto não deve ser nula.")

                .MinimumLength(5)
                .WithMessage("A URL do produto deve ter no mínimo 5 caracteres.");




        }
    }
}
