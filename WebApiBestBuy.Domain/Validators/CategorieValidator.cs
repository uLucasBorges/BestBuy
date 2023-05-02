using System.Data;
using FluentValidation;
using WebApiBestBuy.Domain.Models;

namespace WebApiBestBuy.Domain.Validators
{
    public class CategorieValidator : AbstractValidator<Categorie>
    {
        public CategorieValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("O nome da categoria não pode ser vazio")

                .MinimumLength(5)
                .WithMessage("O nome da categoria deve possuir o minimo de 5 caracteres.");

                RuleFor(x => x.Descricao)
                .NotEmpty()
                .WithMessage("A Descrição da categoria não pode ser vazia.")

                .MinimumLength(20)
                .WithMessage("A Descricaoção da categoria deve possuir o minimo de 20 caracteres.");
        }
    }
}
