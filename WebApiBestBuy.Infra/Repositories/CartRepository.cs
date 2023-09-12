using WebApiBestBuy.Domain.Models;
using Dapper;
using WebApiBestBuy.Domain.ViewModel;
using WebApiBestBuy.Domain.Notifications;
using Microsoft.IdentityModel.Tokens;
using WebApiBestBuy.Infra.Data;
using System.Data;
using WebApiBestBuy.Domain.Interfaces.Repositories;

namespace WebApiBestBuy.Infra.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;
        private readonly INotificationContext _notificationContext;

        public CartRepository(AppDbContext context, INotificationContext notificationContext)
        {
            _context = context;
            _notificationContext = notificationContext;
        }



       
        public async Task<bool> ExistCart(string cartID)
        {
            using (var context = _context)
            {
                _context.Begin();


                var query = "select top 1 1 from cart where Id = @cartID";


                var exec = await context.connection.QueryAsync<Cart>(query, new { cartID } ,transaction: _context.transaction);


                if (!exec.IsNullOrEmpty()) {
                    return true;
                }
               
                _notificationContext.AddNotification(404, "Carrinho inexistente");

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

                context.Dispose();


                if (!productsInCart.Any())
                {
                    _notificationContext.AddNotification(404, "Nenhum Produto encontrado no Carrinho!");
                }

                return new CartVM
                {
                    Products = productsInCart
                 
                };

            }
        }

        public async Task<bool> RemoveProductCart(int ProductId, int AmountInsert, string CartID)
        {
            using (var context = _context)
            {
              
                    _context.Begin();

                   // var query = "exec RemoveOrDeleteCart \r\n@cartId = @CartId,\r\n@productId = @ProductId,\r\n@amountInsert = @AmountInsert";

                    var result = await context.connection.ExecuteAsync(@"[BestBuy].[RemoveOrDeleteCart]", new
                    {
                        cartId = CartID,
                        productId = ProductId,
                        amountInsert = AmountInsert
                    }, transaction: _context.transaction,
                       commandType: CommandType.StoredProcedure);


                context.Commit();


        

                if (result == -100)
                    {
                        _notificationContext.AddNotification(404, "Produto não encontrado");
                        return false;
                    }

                    return true;
                }

            }
        
    
        public async Task InsertOrUpdate(string CartId, int ProductId, double AmountInsert)
        {


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

                return;
            }
        }

    }
}
