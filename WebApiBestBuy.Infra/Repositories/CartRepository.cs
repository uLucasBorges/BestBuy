using WebApiBestBuy.Domain.Interfaces;
using WebApiBestBuy.Domain.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using WebApiBestBuy.Domain.ViewModel;
using WebApiBestBuy.Domain.Notifications;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace WebApiBestBuy.Infra.Repositories
{
    public class CartRepository : ICartRepository
    {
        private string   ConnectionStringEscrita { get; }
        private readonly ICouponRepository _couponRepository;
        private readonly IProductRepository _productRepository;
        private readonly INotificationContext _notificationContext;

        public CartRepository(IOptions<DatabaseConfig> config ,ICouponRepository couponRepository, IProductRepository productRepository, INotificationContext notificationContext)
        {
            ConnectionStringEscrita = config.Value.ConnectionStringEscrita;
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

            using (var connection =  new SqlConnection(ConnectionStringEscrita))
            {
                connection.Open();

                var cart = await ExistCart(CartId);

                if (!cart)
                {
                    _notificationContext.AddNotification(404, "Carrinho não encontrado");
                    return false;
                }

                connection.Open();

                var query = "exec InsertOrUpdateCart \r\n@cartId = @CartId,\r\n@productId = @ProductId,\r\n@amountInsert = @AmountInsert";

                var result = connection.Execute(query, new
                {
                    cartId = CartId,
                    productId = ProductId,
                    amountInsert = AmountInsert
                });

                connection.Dispose();

                if (result == -1) { 
                _notificationContext.AddNotification(404, "Produto não encontrado");
                return false;
                }

                return true;
                }  
            }


        public async Task<bool> ExistCart(string cartID)
        {
            using (var connection =  new SqlConnection(ConnectionStringEscrita))
            {
                connection.Open();

                var query = "select * from cart where Id = @CartID";
                var exec = await connection.QueryAsync<Cart>(query, new { CartID = cartID });
               
                connection.Dispose();

                if (!exec.IsNullOrEmpty())
                    return true;

                   return false;

            }
        }

        public async Task<CartVM> GetProductsByCart(string CartId)
        {
            using (var connection =  new SqlConnection(ConnectionStringEscrita))
            {
                connection.Open();


                var query = "SELECT P.Id, P.Name ,P.Price as unitPrice, C.Quantity , C.ValueTotal \r\n  FROM CART C \r\n  INNER JOIN PRODUCTS P ON P.Id = C.ProductId \r\n  WHERE C.ID = @CartId";

                IEnumerable<ProductViewModel> productsInCart = await connection.QueryAsync<ProductViewModel>(query, new { cartId = CartId });

                var existsCoupon = await _couponRepository.CartHaveCoupon(CartId);
               
                connection.Dispose();

                var controle = 0.0;

               
                    foreach(ProductViewModel obj in productsInCart)
                    {
                        controle += obj.ValueTotal;
                    }

                if (existsCoupon.Success)
                {
                    controle -= existsCoupon.Data.DiscountAmount;

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
            using (var connection =  new SqlConnection(ConnectionStringEscrita))
            {
                connection.Open();
                var cart = await ExistCart(CartID);
                if (cart)
                {
                    var query = "exec RemoveOrDeleteCart \r\n@cartId = @CartId,\r\n@productId = @ProductId,\r\n@amountInsert = @AmountInsert";

                    var result = connection.Execute(query, new
                    {
                        cartId = CartID,
                        productId = ProductId,
                        amountInsert = AmountInsert
                    });

                    connection.Dispose();

                    if (result == -1)
                    {
                        _notificationContext.AddNotification(404, "Produto não encontrado");
                        return false;
                    }

                    return true;
                }

                _notificationContext.AddNotification(404, "Carrinho inexistente");
                return false;

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

            using (var connection = new SqlConnection(ConnectionStringEscrita))
            {
                connection.Open();
                var query = "IF (EXISTS(select top 1 1 from ProductsByCart pbc\r\n  where pbc.id = @CartId\r\n  and pbc.ProductId = @ProductId)) SELECT 1 ELSE SELECT 0";

               var result = (await connection.QueryAsync<int>(query, new {cartId  = CartId, productId = ProductId})).FirstOrDefault();

                if(result == 1)
                query = "update Cart \r\nset Quantity = ((select quantity from ProductsByCart c where c.Id = @cartId and c.ProductId = @productId) + @amountInsert), ValueTotal = (select ValueTotal from cart c where c.Id = @cartId and c.ProductId = @productId)\r\n+ (@amountInsert * (select price from Products where id = @ProductId))\r\nwhere Id = @cartId\r\nand ProductId = @productId";
 

                if(result == 0)
                 query = "INSERT CART VALUES (@CartId,@ProductId,@amountInsert,(@amountInsert * (select price from Products where id = @ProductId)))";
                


                await connection.ExecuteAsync(query, new { cartId = CartId, productId = ProductId, amountInsert = AmountInsert });

                _notificationContext.AddNotification(200, "Inserido com sucesso.");

                return;
            }
        }

    }
}
