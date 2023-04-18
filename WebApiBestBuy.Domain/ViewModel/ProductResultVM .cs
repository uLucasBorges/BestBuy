using WebApiBestBuy.Domain.Models;

namespace WebApiBestBuy.Domain.ViewModel
{
    public class ProductResultVM
    {
        public Product? data { get; set; }
        public bool Success { get; set; }
    }
}
