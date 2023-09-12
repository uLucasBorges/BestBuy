using Dapper;
using WebApiBestBuy.Domain.Models;
using WebApiBestBuy.Domain.ViewModel;
using WebApiBestBuy.Domain.Notifications;
using Microsoft.Extensions.Options;
using Microsoft.Data.SqlClient;
using WebApiBestBuy.Domain.Interfaces.Repositories;

namespace WebApiBestBuy.Infra.Repositories;


public class CategorieRepository : ICategorieRepository
    {
        private readonly string ConnectionStringEscrita;

        private readonly INotificationContext _notificationContext;

        public CategorieRepository(IOptions<DatabaseConfig> config , INotificationContext notificationContext)
        {
            ConnectionStringEscrita = config.Value.ConnectionStringEscrita;
            _notificationContext = notificationContext;
        }

        public async Task CreateCategory(Categorie categorie)
        {
        try {
            using (var connection = new SqlConnection(ConnectionStringEscrita))
            {
                connection.Open();

                var query = @"INSERT INTO [dbo].[CategoriasAprendiz]
                            (CategoriaNome, Descricao) VALUES (@name, @descricao)";

                await connection.ExecuteAsync(query, new { name = categorie.Name, descricao = categorie.Descricao });

                connection.Dispose();
            }
            } catch (Exception e)
        {
            _notificationContext.AddNotification(500, e.Message);
        }
        }

        public async Task<ResultViewModel> GetCategorie(string categoriaNome)
        {
            using (var connection =  new SqlConnection(ConnectionStringEscrita))
            {
                var query = @"SELECT *  FROM [clearsale].[dbo].[CategoriasAprendiz]
                             WHERE CategoriaNome = @categoriaNome";
                var result = (await connection.QueryAsync<Categorie>(query, new { CategoriaNome = categoriaNome })).FirstOrDefault();
                connection.Dispose();

            if (result != null)

             return new ResultViewModel
             {
                 Data = result,
                 Success = true
             };


            _notificationContext.AddNotification(404, "Categoria não encontrada");
            return new ResultViewModel();
            }
        }

        public async Task DeleteCategory(int id)
        {
           
            try
            {
                using (var connection =  new SqlConnection(ConnectionStringEscrita))
                {
                    var query = @"DELETE FROM [dbo].[CategoriasAprendiz]
                                  WHERE CategoriaId =  @id";

                    await connection.ExecuteAsync(query, new
                    {
                        Id = id
                    });

                  connection.Dispose();

                }
            } catch (Exception e)
            {
                  _notificationContext.AddNotification(500, e.Message);
            }
    }
    }

