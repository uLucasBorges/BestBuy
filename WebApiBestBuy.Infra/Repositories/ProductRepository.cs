﻿using WebApiBestBuy.Domain.Interfaces;
using WebApiBestBuy.Domain.Notifications;
using Dapper;
using Microsoft.Data.SqlClient;
using WebApiBestBuy.Infra.Data;
using WebApiBestBuy.Domain.Models;
using WebApiBestBuy.Domain.ViewModel;
using Microsoft.Extensions.Options;

namespace WebApiBestBuy.Infra.Repositories;

    public class ProductRepository : IProductRepository
    {
        private string ConnectionStringEscrita { get; }
        private readonly INotificationContext _notificationContext;


        public ProductRepository(AppDbContext context,IOptions<DatabaseConfig> config, INotificationContext notificationContext)
        {
            ConnectionStringEscrita = config.Value.Clearsale_Write;
            _notificationContext = notificationContext;
        }

        public async Task<Product> CreateProduct(Product product)
        {
        using (var connection = new SqlConnection(ConnectionStringEscrita))
        {
            var query = "INSERT INTO Products (Name,Price,Description,CategoryId,ImageURL) VALUES (@name,@price,@description,@categoryId,@imageURL)";
            var result = await connection.ExecuteAsync(query,
                new
                {
                    name = product.Name,
                    price = product.Price,
                    description = product.Description,
                    categoryId = product.CategoryId,
                    imageurl = product.ImageUrl
                });

            connection.Dispose();

            if (result == -1)
                _notificationContext.AddNotification(400, "Erro ao criar o produto");

            else
                _notificationContext.AddNotification(201, "Produto criado com sucesso!");

        }   

            return product;

        }

        public async Task<bool> DeleteProduct(int id)
        {
           var existsProduct = await GetProduct(id);

           if(!existsProduct.Success)
            {
                _notificationContext.AddNotification(404, "Produto inexistente");
                return false;
            }

            using (var connection =  new SqlConnection(ConnectionStringEscrita))
            {
                var query = "DELETE FROM [dbo].[Products]  WHERE Id = @id";
               
                await connection.ExecuteAsync(query, new
                {
                    Id = id
                });

            connection.Dispose();


            return true;
            }
             
        }

        public async Task<ProductResultVM> GetProduct(int Id)
        {
            using (var connection =  new SqlConnection(ConnectionStringEscrita))
            {
                var query = "SELECT * FROM Products WHERE id = @Id";
                var result = (await connection.QueryAsync<Product>(query, new { id = Id })).FirstOrDefault() ;

            connection.Dispose();

            if (result != null)
    
                     return new ProductResultVM
                    {
                        data = result,
                        Success = true
                    };


                   _notificationContext.AddNotification(404, "Produto não encontrado");
                   return new ProductResultVM();
            }
        }

        public async Task<ResultViewModel> GetProducts()
        {
            using (var connection =  new SqlConnection(ConnectionStringEscrita))
            {
                var query = "SELECT * FROM Products";
                var result = (await connection.QueryAsync<Product>(query)).ToList();
                connection.Dispose();

            if (result.Any())

                    return new ResultViewModel
                    {
                        data = result,
                        Success = true
                    };

                _notificationContext.AddNotification(404, "Não há produtos");
                return new ResultViewModel();
            }
        }

        public async Task<ResultViewModel> UpdateProduct(Product product)
        {
            var existsProduct = await GetProduct(product.Id);

            if(existsProduct.Success)
            {
                using (var connection =  new SqlConnection(ConnectionStringEscrita))
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
                        name = product.Name ?? existsProduct.data.Name,
                        price = product.Price == 0.0 ? existsProduct.data.Price : product.Price,
                        description = product.Description ?? existsProduct.data.Description,
                        categoryId = existsProduct.data.CategoryId == 0 ?  product.CategoryId : existsProduct.data.CategoryId,
                        imageUrl = product.ImageUrl ?? existsProduct.data.ImageUrl

                    }) ;;
                connection.Dispose();

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

