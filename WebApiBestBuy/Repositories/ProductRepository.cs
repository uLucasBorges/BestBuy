using BestBuy.Core.Interfaces;
using BestBuy.Core.Notification;
using Dapper;
using Microsoft.Data.SqlClient;
using WebApiBestBuy.Models;
using WebApiBestBuy.ViewModel;

namespace BestBuy.Infra.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IConfiguration _config;
        private readonly INotificationContext _notificationContext;
        private readonly string stringConexao;


        public ProductRepository(IConfiguration config, INotificationContext notificationContext)
        {
            _notificationContext = notificationContext;
            stringConexao = config.GetSection("ConnectionStrings").GetSection("Default").Value;
        }


        public async Task<Product> CreateProduct(Product product)
        {
            using (var connection = new SqlConnection(stringConexao))
            {
               var query = "INSERT INTO Products (Name,Price,Description,CategoryId,ImageURL) VALUES (@name,@price,@description,@categoryId,@imageURL)";
               var result =  await connection.ExecuteAsync(query,
                   new {name = product.Name,
                       price = product.Price,
                       description = product.Description,
                       categoryId = product.CategoryId, 
                       imageurl = product.ImageUrl });
            }

            return product;

        }

        public Task<bool> DeleteProduct(int ProducId)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultViewModel> GetProduct(int Id)
        {
            using(var connection = new SqlConnection(stringConexao))
            {
                var query = "SELECT * FROM Products WHERE id = @Id";
                var result = (await connection.QueryAsync<Product>(query, new { id = Id })).FirstOrDefault() ;

                if (result.IsValid)

                    return new ResultViewModel
                    {
                        data = result,
                        Success = true
                    };

                return new ResultViewModel { Success = false };
            }
        }

        public async Task<ResultViewModel> GetProducts()
        {
            using (var connection = new SqlConnection(stringConexao))
            {
                var query = "SELECT * FROM Products";
                var result = (await connection.QueryAsync<Product>(query)).ToList();

                if (result.Any())

                    return new ResultViewModel
                    {
                        data = result,
                        Success = true
                    };

                return new ResultViewModel { Success = false };
            }
        }

        public async Task<ResultViewModel> UpdateProduct(Product product)
        {
            var existsProduct = await GetProduct(product.Id);

            if(existsProduct.Success)
            {
                using(var connection = new SqlConnection(stringConexao))
                {
                    var query = @"UPDATE Products
                       SET
                       Name = @name,
                       Price = @price,
                       Description = @description,
                       CategoryId = @categoryId,
                       ImageURL = @imageUrl
                       WHERE id = @id";

                    var result = await connection.ExecuteAsync(query, new
                    {
                        id = product.Id,
                        name = product.Name,
                        price = product.Price,
                        description = product.Description,
                        categoryId = product.CategoryId,
                        imageUrl = product.ImageUrl
                        
                    });;

                    if (result == -1)
                    {
                        _notificationContext.AddNotification(400, "não foi possivel atualizar o produto");
                        return new ResultViewModel { Success = false, data = product };
                    }
                    else
                        return new ResultViewModel { Success = true, data = product };
                }
            }

            _notificationContext.AddNotification(404, "Produto não encontrado");

            return new ResultViewModel { Success = false , data = product};
            
        }
    }
}
