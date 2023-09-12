using WebApiBestBuy.Domain.Models;
using WebApiBestBuy.Domain.ViewModel;

namespace WebApiBestBuy.Domain.Interfaces.Repositories;

public interface ICouponRepository
{
    public Task<Coupon> CreateCoupon(Coupon coupon);
    public Task<bool> DeleteCoupon(string couponCode);
    public Task<bool> ExistsCoupon(string couponCode);
    public Task<bool> CartHaveCoupon(string CartId);
    public Task<bool> ApplyCoupon(string cartId, string couponCode);
    public Task<bool> RemoveCoupon(string cartId);
    public Task<Coupon> GetCouponByCart(string couponCode);




}

