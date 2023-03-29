
namespace WebApiBestBuy.Models
{
    public class Cart 
    {
        public string CartID { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double ValueTotal { get; set; }
    }
}
