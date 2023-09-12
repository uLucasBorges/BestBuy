using WebApiBestBuy.Domain.Models;
using WebApiBestBuy.Domain.ViewModel;

namespace WebApiBestBuy.Domain.Interfaces.Repositories;

public interface ICartRepository
{
    public Task<bool> ExistCart(string CartId);
    public Task<bool> RemoveProductCart(int productId, int quantity, string cartId);
    public Task<CartVM> GetProductsByCart(string cartId);

    public Task InsertOrUpdate(string CartId, int ProductId, double AmountInsert);


}
