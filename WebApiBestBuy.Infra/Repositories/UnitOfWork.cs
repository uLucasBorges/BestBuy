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

        protected readonly DatabaseConfig _dbConfig;

        public UnitOfWork(IOptions<DatabaseConfig> config)
        {
            _id = Guid.NewGuid();
            _dbConfig = config.Value;
        }

        IDbConnection _connection = null;
        IDbTransaction _transaction = null;
        Guid _id = Guid.Empty;

        IDbConnection IUnitOfWork.Connection
        {
            get { return _connection; }
        }
        IDbTransaction IUnitOfWork.Transaction
        {
            get { return _transaction; }
        }
        Guid IUnitOfWork.Id
        {
            get { return _id; }
        }

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
            if (_connection == null)
                return;

            if (_transaction != null)
                _transaction.Commit();

            _connection.Close();
            Dispose();
        }

        public void Rollback()
        {
            if (_connection == null)
                return;

            if (_transaction != null)
                _transaction.Rollback();

            _connection.Close();
            Dispose();
        }

        public void Dispose()
        {
            if (_connection == null)
                return;

            if (_transaction != null)
                _transaction.Dispose();

            _transaction = null;
            _connection.Close();
        }

    }


    public interface IUnitOfWork : IDisposable
    {
          ICartRepository _cartRepository { get; set; }
          ICategorieRepository _categorieRepository { get; set; }
          ICouponRepository _couponRepository { get; set; }
          IProductRepository _ProductRepository { get; set; }


        Guid Id { get; }
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }
        void Begin();
        void Commit();
        void Rollback();
    }
}

