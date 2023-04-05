using BestBuy.Core.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using WebApiBestBuy.Models;

namespace BestBuy.Infra.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IConfiguration _config;
        private readonly string stringConexao;


        public ProductRepository(IConfiguration config)
        {
            stringConexao = config.GetSection("ConnectionStrings").GetSection("Default").Value;
        }


        public async Task<Product> CreateProduct(Product product)
        {
            using (var connection = new SqlConnection(stringConexao))
            {
               var query = "  INSERT INTO Product VALUES ('A',100.0,'AA',10,'AA')";
               var result =  await connection.ExecuteAsync(query, new { });
            }

            return product;

        }

        public Task<bool> DeleteProduct(int ProducId)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProduct(int ProducId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetProducts()
        {
            throw new NotImplementedException();
        }

        public Task<Product> UpdateProduct(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
