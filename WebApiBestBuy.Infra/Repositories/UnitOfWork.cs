using WebApiBestBuy.Domain.Interfaces;
using WebApiBestBuy.Domain.Interfaces.Repositories;
using WebApiBestBuy.Domain.Notifications;

namespace WebApiBestBuy.Infra.Repositories
{

    public class UnitOfWork : IUnitOfWork
    {

        private ICartRepository _cartRepository;
        private ICategorieRepository _categorieRepository;
        private ICouponRepository _couponRepository;
        private IProductRepository _productRepository;
        private INotificationContext _notificationContext;

        public UnitOfWork( ICartRepository cartRepository, ICategorieRepository categorieRepository, ICouponRepository couponRepository, IProductRepository productRepository, INotificationContext notificationContext)
        {
            _cartRepository = cartRepository;
            _categorieRepository = categorieRepository;
            _couponRepository = couponRepository;
            _productRepository = productRepository;
            _notificationContext = notificationContext;
        }



        public ICartRepository CartRepository => _cartRepository;
        public ICategorieRepository CategorieRepository => _categorieRepository;
        public ICouponRepository CouponRepository => _couponRepository;
        public IProductRepository ProductRepository => _productRepository;

    }
}

