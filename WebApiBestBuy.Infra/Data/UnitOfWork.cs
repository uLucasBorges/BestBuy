using Core.Interfaces.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;


namespace WebApiBestBuy.Infra.Data;

//Deve ser injetada como Scoped
//A Classe não está seno utilizada
public class UnitOfWork : IUnitOfWork
{

    protected readonly DatabaseConfig _dbConfig;

    public UnitOfWork(IOptions<DatabaseConfig> config)
    {
        _dbConfig = config.Value;
    }

    IDbConnection _connection = null;
    IDbTransaction _transaction = null;
    Guid _id = Guid.Empty;


    public void Begin()
    {
        if (_connection != null)
            return;

        _connection = new SqlConnection(_dbConfig.Clearsale_Write);
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
