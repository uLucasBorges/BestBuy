
using WebApiBestBuy.Domain.Entities;
using WebApiBestBuy.Domain.Validators;

namespace WebApiBestBuy.Domain.Models;

public class Categorie : EntityBase
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Descricao { get; set; }

    public Categorie()
    {
        _errors = new List<string>();
    }

    public bool Validate() => base.Validate(new CategorieValidator(), this);

}
