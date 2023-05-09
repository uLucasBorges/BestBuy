using WebApiBestBuy.Domain.Interfaces;
using Dapper;
using WebApiBestBuy.Domain.Models;
using WebApiBestBuy.Domain.ViewModel;
using WebApiBestBuy.Domain.Notifications;
using Microsoft.Extensions.Options;
using Microsoft.Data.SqlClient;

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

        public async Task<Categorie> CreateCategory(Categorie categorie)
        {
            using (var connection =  new SqlConnection(ConnectionStringEscrita))
            {
              connection.Open();

            var query = "INSERT INTO [dbo].[CategoriasAprendiz] (CategoriaNome, Descricao) VALUES (@name, @descricao)";
                var result = await connection.ExecuteAsync(query, new { name = categorie.Name, descricao = categorie.Descricao });

            connection.Dispose();

            if (result == -1)
                    _notificationContext.AddNotification(400, "Erro ao criar a categoria");

                else
                    _notificationContext.AddNotification(201, "categoria criada com sucesso!");

                return categorie;
            }
        }

        public async Task<ResultViewModel> GetCategorie(int Id)
        {
            using (var connection =  new SqlConnection(ConnectionStringEscrita))
            {
                var query = "SELECT *  FROM [clearsale].[dbo].[CategoriasAprendiz]\r\n  WHERE CategoriaId = @Id";
                var result = (await connection.QueryAsync<Categorie>(query, new { id = Id })).FirstOrDefault();
                connection.Dispose();

            if (result != null)

                    return new ResultViewModel
                    {
                        Data = result,
                        Success = true
                    };


                _notificationContext.AddNotification(404, "Categoria não encontrado");
                return new ResultViewModel();
            }
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var existsProduct = await GetCategorie(id);

            if (!existsProduct.Success)
            {
                _notificationContext.AddNotification(404, "Categoria inexistente");
                return false;
            }
            try
            {
                using (var connection =  new SqlConnection(ConnectionStringEscrita))
                {
                    var query = "DELETE FROM [dbo].[CategoriasAprendiz]\r\n      WHERE CategoriaId =  @id";

                    await connection.ExecuteAsync(query, new
                    {
                        Id = id
                    });
                connection.Dispose();


                return true;
                }
            } catch (Exception ex)
            {
                _notificationContext.AddNotification(400, "existe carrinhos com esse produto");
                return false;
            }
        }
    }

