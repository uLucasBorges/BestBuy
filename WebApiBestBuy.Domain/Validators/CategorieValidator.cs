using FluentValidation;
using WebApiBestBuy.Domain.Models;

namespace WebApiBestBuy.Domain.Validators
{
    public class CategorieValidator : AbstractValidator<Categorie>
    {
        public CategorieValidator()
        {


            RuleFor(x => x.Name).NotNull().WithMessage("O nome da categoria não pode ser vazio");

            RuleFor(x => x.Name).MinimumLength(5).WithMessage("O nome da categoria deve possuir o minimo de 5 caracteres.");

            RuleFor(x => x.Descricao).NotNull().WithMessage("A Descrição da categoria não pode ser vazia.");

            RuleFor(x => x.Descricao).MinimumLength(20).WithMessage("A Descricaoção da categoria deve possuir o minimo de 20 caracteres.");

            RuleFor(x => x.Name).Equal(x => x.Descricao).WithMessage("O Nome e categoria devem ser iguais!");

        }
    }
}
