

using System.Data;

namespace WebApiBestBuy.Domain.Interfaces
{
    public interface IUnitOfWork { 

        public ICartRepository CartRepository { get; }
        public ICategorieRepository CategorieRepository { get; }
        public ICouponRepository CouponRepository { get; }
        public IProductRepository ProductRepository { get; }
     
    }
}
