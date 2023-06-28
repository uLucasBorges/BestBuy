using WebApiBestBuy.Domain.Interfaces;
using WebApiBestBuy.Domain.Models;
using Dapper;
using WebApiBestBuy.Domain.ViewModel;
using WebApiBestBuy.Domain.Notifications;
using Microsoft.IdentityModel.Tokens;
using WebApiBestBuy.Infra.Data;
using System.Data;

namespace WebApiBestBuy.Infra.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;
        private readonly ICouponRepository _couponRepository;
        private readonly IProductRepository _productRepository;
        private readonly INotificationContext _notificationContext;

        public CartRepository(AppDbContext context, ICouponRepository couponRepository, IProductRepository productRepository, INotificationContext notificationContext)
        {
            _context = context;
            _productRepository = productRepository;
            _notificationContext = notificationContext;
            _couponRepository = couponRepository;
        }

        public async Task<bool> AddCoupon(string cartId, string couponCode)
        {
            var cartExists = await this.ExistCart(cartId);

            if (cartExists)
            {
                var coupon = await _couponRepository.ExistsCoupon(couponCode);

                if (coupon.Success)
                {
                    await _couponRepository.ApplyCoupon(cartId, couponCode);

                    return true;
                }
            }

            return false;


        }

        //metodo não está sendo utilizado, pois estou tirando a dependencia da proc e crie o metoddo InsertOrUpdate com a lógica
        public async Task<bool> AddProductCart(string CartId, int ProductId, int AmountInsert)
        {

            using (var context =  _context)
            {
                var cart = await ExistCart(CartId);

                if (!cart)
                {
                    _notificationContext.AddNotification(404, "Carrinho não encontrado");
                    return false;
                }

                //var query = "exec InsertOrUpdateCart \r\n@cartId = @CartId,\r\n@productId = @ProductId,\r\n@amountInsert = @AmountInsert";

                _context.Begin();

                var result = await context.connection.ExecuteAsync(@"InsertOrUpdateCart", new
                {
                    cartId = CartId,
                    productId = ProductId,
                    amountInsert = AmountInsert
                }, transaction: _context.transaction,
                commandType: CommandType.StoredProcedure);

                _context.Commit();


                if (result == -1) { 
                _notificationContext.AddNotification(404, "Produto não encontrado");
                return false;

                }

                return true;
                }  
            }

        /// <summary>
        ///  Verifica se o carrinho existe
        /// </summary>
        /// <param name="cartID"></param>
        /// <returns> Retorna true caso exista, false caso não exista</returns>
        public async Task<bool> ExistCart(string cartID)
        {
            using (var context = _context)
            {
                _context.Begin();


                var query = "select top 1 1 from cart where Id = @cartID";


                var exec = await context.connection.QueryAsync<Cart>(query, new { cartID } ,transaction: _context.transaction);
               

                if (!exec.IsNullOrEmpty())
                    return true;

                   return false;

            }
        }

        public async Task<CartVM> GetProductsByCart(string CartId)
        {
            using (var context = _context)
            {

                _context.Begin();

                var query = "SELECT P.Id, P.Name ,P.Price as unitPrice, C.Quantity , C.ValueTotal \r\n  FROM CART C \r\n  INNER JOIN PRODUCTS P ON P.Id = C.ProductId \r\n  WHERE C.ID = @CartId";

                IEnumerable<ProductViewModel> productsInCart = await context.connection.QueryAsync<ProductViewModel>(query, new { CartId }, transaction: _context.transaction);

                var existsCoupon = await _couponRepository.CartHaveCoupon(CartId);
               
                context.Dispose();

                var controle = 0.0;

               
                foreach(ProductViewModel obj in productsInCart)
                {
                        controle += obj.ValueTotal;
                }

               var teste = productsInCart.Sum(x => x.ValueTotal);
                //verificar se é o mesmo resultado do foreach

              )

                if (existsCoupon.Success)
                {
                    controle -=existsCoupon.Data.DiscountAmount;

                    if (controle < 0)
                        controle = 0;
                }


                return new CartVM
                {
                    Products = productsInCart,
                    DiscountAmount = existsCoupon.Data.DiscountAmount,
                    CartPriceWithDiscount = controle 
                };

                
            }
        }

        public async Task<bool> RemoveProductCart(int ProductId, int AmountInsert, string CartID)
        {
            using (var context = _context)
            {
              
                    _context.Begin();

                   // var query = "exec RemoveOrDeleteCart \r\n@cartId = @CartId,\r\n@productId = @ProductId,\r\n@amountInsert = @AmountInsert";

                    var result = await context.connection.ExecuteAsync(@"RemoveOrDeleteCart", new
                    {
                        cartId = CartID,
                        productId = ProductId,
                        amountInsert = AmountInsert
                    }, transaction: _context.transaction, commandType: CommandType.StoredProcedure);


                context.Commit();


        

                if (result == -1)
                    {
                        _notificationContext.AddNotification(404, "Produto não encontrado");
                        return false;
                    }

                    return true;
                }

            }
        
        /// <summary>
        /// Método para inserir ou adicionar mais produtos ao carrinho
        /// </summary>
        /// <param name="CartId"></param>
        /// <param name="ProductId"></param>
        /// <param name="AmountInsert"></param>
        /// <returns></returns>
        public async Task InsertOrUpdate(string CartId, int ProductId, double AmountInsert)
        {

            var exists = await _productRepository.GetProduct(ProductId);

    
            if (_notificationContext.HasNotifications())
            {
                return;
            }

            using (var context = _context)
            {
                _context.Begin();

                var query = "IF (EXISTS(select top 1 1 from ProductsByCart pbc\r\n  where pbc.id = @CartId\r\n  and pbc.ProductId = @ProductId)) SELECT 1 ELSE SELECT 0";

                var result = (await context.connection.QueryAsync<int>(query, new {cartId  = CartId, productId = ProductId }, transaction: _context.transaction)).FirstOrDefault();

                if(result == 1)
                query = "update Cart \r\nset Quantity = ((select quantity from ProductsByCart c where c.Id = @cartId and c.ProductId = @productId) + @amountInsert), ValueTotal = (select ValueTotal from cart c where c.Id = @cartId and c.ProductId = @productId)\r\n+ (@amountInsert * (select price from Products where id = @ProductId))\r\nwhere Id = @cartId\r\nand ProductId = @productId";
 

                if(result == 0)
                 query = "INSERT CART VALUES (@CartId,@ProductId,@amountInsert,(@amountInsert * (select price from Products where id = @ProductId)))";


                await context.connection.ExecuteAsync(query, new {CartId, ProductId, AmountInsert }, transaction: _context.transaction);
                
                context.Commit();

                _notificationContext.AddNotification(200, "Inserido com sucesso.");

                return;
            }
        }

    }
}
