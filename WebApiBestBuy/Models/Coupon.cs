
using Newtonsoft.Json;

namespace WebApiBestBuy.Models
{
    public class Coupon 
    {
        public string CouponCode { get; set; }

        public double DiscountAmount { get; set; }
    }
}
