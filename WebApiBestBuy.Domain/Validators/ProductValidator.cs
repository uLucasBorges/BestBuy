
using FluentValidation;
using WebApiBestBuy.Domain.Models;

namespace WebApiBestBuy.Domain.Validators;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("O nome do produto não pode ser vazio.");

        RuleFor(x => x.Name)
            .NotNull()
            .WithMessage("o nome do produto não deve ser nulo.");
                  
            
        RuleFor(x => x.Name)
            .MinimumLength(5)
            .WithMessage("O nome do produto deve ter no mínimo 5 caracteres.");
        
        RuleFor(x => x.Name)
            .MaximumLength(50)
            .WithMessage("O nome do produto ter no máximo 50 caracteres."); ;


        RuleFor(x => x.CategoryId)
            .NotEqual(0)
            .WithMessage("A Categoria do produto não pode ser 0");
   
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Você deve possuir uma categoria");
       
        RuleFor(x => x.CategoryId)
            .NotNull()
            .WithMessage("A categoria não pode ser nula");


        RuleFor(x => x.Price)
        .NotEqual(0)
        .WithMessage("O Valor produto não pode ser $0,00");
        
        RuleFor(x => x.Price)
        .NotEmpty()
        .WithMessage("O Valor do produto não pode ser vazio.");
        
        RuleFor(x => x.Price)
        .NotNull()
        .WithMessage("O Valor do produto não deve ser nulo.");



        RuleFor(X => X.Description)
            .NotEmpty()
            .WithMessage("A Descrição do produto não pode ser vazia.");
    
        RuleFor(X => X.Description)
            .NotNull()
            .WithMessage("A Descrição do produto não deve ser nula.");

        RuleFor(X => X.Description)
            .MinimumLength(5)
            .WithMessage("A Descrição do produto deve ter no mínimo 5 caracteres.");

        RuleFor(X => X.Description)
            .MaximumLength(50)
            .WithMessage("A Descrição do produto deve ter no máximo 50 caracteres.");


        RuleFor(x => x.ImageUrl)
            .NotEmpty()
            .WithMessage("A URL do produto não pode ser vazia.")

            .NotNull()
            .WithMessage("A URL do produto não deve ser nula.")

            .MinimumLength(5)
            .WithMessage("A URL do produto deve ter no mínimo 5 caracteres.");




    }
}
