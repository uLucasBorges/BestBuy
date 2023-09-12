

using System.Data;
using WebApiBestBuy.Domain.Interfaces.Repositories;

namespace WebApiBestBuy.Domain.Interfaces
{
    public interface IUnitOfWork { 

        public ICartRepository CartRepository { get; }
        public ICategorieRepository CategorieRepository { get; }
        public ICouponRepository CouponRepository { get; }
        public IProductRepository ProductRepository { get; }
     
    }
}
