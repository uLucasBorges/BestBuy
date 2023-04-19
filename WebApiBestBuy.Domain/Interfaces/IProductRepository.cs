﻿using WebApiBestBuy.Domain.Models;
using WebApiBestBuy.Domain.ViewModel;

namespace WebApiBestBuy.Domain.Interfaces;

    public interface IProductRepository
    {

        public Task<Product> CreateProduct(Product product);
        public Task<bool> DeleteProduct(int ProducId);
        public Task<ProductResultVM> GetProduct(int ProducId);
        public Task<ResultViewModel> GetProducts();
        public Task<ResultViewModel> UpdateProduct(Product product);
     
    }
