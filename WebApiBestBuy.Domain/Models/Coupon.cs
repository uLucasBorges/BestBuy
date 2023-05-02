

using WebApiBestBuy.Domain.Entities;
using WebApiBestBuy.Domain.Validators;

namespace WebApiBestBuy.Domain.Models;

public class Coupon : EntityBase
{
    public int Id { get; set; }
    public string CouponCode { get; set; }

    public double DiscountAmount { get; set; }

    public Coupon()
    {
        _errors = new List<string>();
    }

    public bool Validate() => base.Validate(new CouponValidator(), this);

}
