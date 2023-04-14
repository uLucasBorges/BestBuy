using BestBuy.Core.Interfaces;
using BestBuy.Core.Notification;
using Dapper;
using WebApiBestBuy.Data;
using WebApiBestBuy.Models;
using WebApiBestBuy.ViewModel;

namespace BestBuy.Infra.Repositories
{
    public class CategorieRepository : ICategorieRepository
    {
        private readonly AppDbContext _context;
        private readonly INotificationContext _notificationContext;

        public CategorieRepository(AppDbContext context, INotificationContext notificationContext)
        {
            _context = context;
            _notificationContext = notificationContext;
        }

        public async Task<Categorie> CreateCategory(Categorie categorie)
        {
            using (var connection = _context.Connect())
            {
                var query = "INSERT INTO [dbo].[CategoriasAprendiz] (CategoriaNome, Descricao) VALUES (@name, @descricao)";
                var result = await connection.ExecuteAsync(query, new { name = categorie.Name, descricao = categorie.Descricao });

                if (result == -1)
                    _notificationContext.AddNotification(400, "Erro ao criar a categoria");

                else
                    _notificationContext.AddNotification(201, "categoria criada com sucesso!");

                return categorie;
            }
        }

        public async Task<ResultViewModel> GetCategorie(int Id)
        {
            using (var connection = _context.Connect())
            {
                var query = "SELECT *  FROM [clearsale].[dbo].[CategoriasAprendiz]\r\n  WHERE CategoriaId = @Id";
                var result = (await connection.QueryAsync<Categorie>(query, new { id = Id })).FirstOrDefault();

                if (result != null)

                    return new ResultViewModel
                    {
                        data = result,
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
                using (var connection = _context.Connect())
                {
                    var query = "DELETE FROM [dbo].[CategoriasAprendiz]\r\n      WHERE CategoriaId =  @id";

                    await connection.ExecuteAsync(query, new
                    {
                        Id = id
                    });

                    return true;
                }
            } catch (Exception ex)
            {
                _notificationContext.AddNotification(400, "existe carrinhos com esse produto");
                return false;
            }
        }
    }
}
