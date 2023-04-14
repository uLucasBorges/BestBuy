﻿using BestBuy.Core.Interfaces;
using BestBuy.Core.Notification;
using Dapper;
using Microsoft.Data.SqlClient;
using WebApiBestBuy.Data;
using WebApiBestBuy.Models;
using WebApiBestBuy.ViewModel;

namespace BestBuy.Infra.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly INotificationContext _notificationContext;

        public ProductRepository(AppDbContext context, INotificationContext notificationContext)
        {
            _context = context;
            _notificationContext = notificationContext;
        }

        public async Task<Product> CreateProduct(Product product)
        {
            using (var connection = _context.Connect())
            {
               var query = "INSERT INTO Products (Name,Price,Description,CategoryId,ImageURL) VALUES (@name,@price,@description,@categoryId,@imageURL)";
               var result =  await connection.ExecuteAsync(query,
                   new {name = product.Name,
                       price = product.Price,
                       description = product.Description,
                       categoryId = product.CategoryId, 
                       imageurl = product.ImageUrl });

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

            using (var connection = _context.Connect())
            {
                var query = "DELETE FROM [dbo].[Products]  WHERE Id = @id";
               
                await connection.ExecuteAsync(query, new
                {
                    Id = id
                });

                return true;
            }
             
        }

        public async Task<ProductResultVM> GetProduct(int Id)
        {
            using (var connection = _context.Connect())
            {
                var query = "SELECT * FROM Products WHERE id = @Id";
                var result = (await connection.QueryAsync<Product>(query, new { id = Id })).FirstOrDefault() ;

                    if(result != null)
    
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
            using (var connection = _context.Connect())
            {
                var query = "SELECT * FROM Products";
                var result = (await connection.QueryAsync<Product>(query)).ToList();

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
                using (var connection = _context.Connect())
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
