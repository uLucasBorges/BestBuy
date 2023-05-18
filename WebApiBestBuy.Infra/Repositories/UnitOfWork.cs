using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using WebApiBestBuy.Domain.Interfaces;

namespace WebApiBestBuy.Infra.Repositories
{
    //Deve ser injetada como Scoped

    public class UnitOfWork : IUnitOfWork
    {

        private readonly DatabaseConfig _dbConfig;
        private ICartRepository _cartRepository;
        private ICategorieRepository _categorieRepository;
        private ICouponRepository _couponRepository;
        private IProductRepository _productRepository;

        IDbConnection _connection;
        IDbTransaction _transaction;
        Guid _id = Guid.Empty;

        public UnitOfWork(IOptions<DatabaseConfig> config, ICartRepository cartRepository, ICategorieRepository categorieRepository, ICouponRepository couponRepository, IProductRepository productRepository)
        {
            _dbConfig = config.Value;
            _cartRepository = cartRepository;
            _categorieRepository = categorieRepository;
            _couponRepository = couponRepository;
            _productRepository = productRepository;
            _id = Guid.NewGuid();
        }



        public ICartRepository CartRepository => _cartRepository;
        public ICategorieRepository CategorieRepository => _categorieRepository;
        public ICouponRepository CouponRepository => _couponRepository;
        public IProductRepository ProductRepository => _productRepository;



        IDbConnection IUnitOfWork.Connection   { get;}
        IDbTransaction IUnitOfWork.Transaction { get; }
        Guid IUnitOfWork.Id{ get; }


        public void Begin()
        {
            if (_connection != null)
                return;

            _connection = new SqlConnection(_dbConfig.ConnectionStringEscrita);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public void Commit()
        {
            _transaction?.Commit();

            _connection?.Close();
            Dispose();
        }

        public void Rollback()
        {
     
            _transaction?.Rollback();

            _connection?.Close();
            Dispose();
        }

        public void Dispose()
        {
        
                _transaction?.Dispose();
                _connection?.Close();
        }

    }


    public interface IUnitOfWork : IDisposable
    { 

        public ICartRepository CartRepository { get; }
        public ICategorieRepository CategorieRepository { get; }
        public ICouponRepository CouponRepository { get; }
        public IProductRepository ProductRepository { get; }
        public Guid Id { get; }
        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; }


        void Begin();
        void Commit();
        void Rollback();
    }
}

