using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiBestBuy.Domain.ViewModel;

namespace WebApiBestBuy.Domain.Interfaces.Services
{
    public interface ICartService
    {

        public Task<bool> RemoveProductCart(int productId, int quantity, string cartId);
        public Task<CartVM> GetProductsByCart(string cartId);
        public Task AddCoupon(string cartId, string couponCode);
        public Task InsertOrUpdate(string CartId, int ProductId, double AmountInsert = 1);

    }
}
