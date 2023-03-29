using WebApiBestBuy.Models;
using WebApiBestBuy.ViewModel;

namespace BestBuy.Core.Interfaces
{
    public interface ICartRepository
    {
        public Task<bool> ExistCart(string CartId);
        public Task<bool> AddProductCart(string CartId, int productId, int quantity = 1);
        public Task<bool> RemoveProductCart(int productId , string cartId);
        public Task<CartVM> GetProductsByCart(string cartId);
        public Task<bool> AddCoupon(string cartId, string couponCode);

    }
}
