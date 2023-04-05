

using BestBuy.Entities;
using WebApiBestBuy.Validators;

namespace WebApiBestBuy.Models
{
    public class Product : EntityBase
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public string Description { get; set; }
        public int CategoryId { get; set; }

        public string ImageUrl { get; set; }

        public Product(string id, string name, double price, string description, int categoryId, string imageUrl)
        {
            Id = id;
            Name = name;
            Price = price;
            Description = description;
            CategoryId = categoryId;
            ImageUrl = imageUrl;
            _errors = new List<string>();

        }

        public bool Validate() => base.Validate(new ProductValidator(), this);

    }
}
