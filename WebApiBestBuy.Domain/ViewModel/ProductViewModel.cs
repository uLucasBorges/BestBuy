using System.Text.Json.Serialization;

namespace WebApiBestBuy.Domain.ViewModel;

public class ProductViewModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public double unitPrice { get; set; }
    public int Quantity { get; set; }
    public double ValueTotal { get; set; }

}
