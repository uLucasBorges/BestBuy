namespace WebApiBestBuy.Domain.ViewModel;

public class CartVM
{
    public IEnumerable<ProductViewModel> Products { get; set; }
    public double DiscountAmount { get; set; }
    public double CartPriceWithDiscount { get; set; }
}
