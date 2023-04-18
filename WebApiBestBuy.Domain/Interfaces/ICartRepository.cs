using WebApiBestBuy.Domain.Models;
using WebApiBestBuy.Domain.ViewModel;

namespace WebApiBestBuy.Domain.Interfaces;

public interface ICartRepository
{
    public Task<bool> ExistCart(string CartId);
    public Task<bool> AddProductCart(string CartId, int productId, int quantity = 1);
    public Task<bool> RemoveProductCart(int productId , int quantity, string cartId);
    public Task<CartVM> GetProductsByCart(string cartId);
    public Task<bool> AddCoupon(string cartId, string couponCode);

}
