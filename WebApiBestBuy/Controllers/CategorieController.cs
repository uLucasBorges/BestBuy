using WebApiBestBuy.Domain.Notifications;
using Microsoft.AspNetCore.Mvc;
using WebApiBestBuy.Domain.Interfaces;
using WebApiBestBuy.Domain.Models;

namespace WebApiBestBuy.Api.Controllers
{
    [Route("[Controller]")]
    public class CategorieController : BaseController
    {
        private readonly ICategorieRepository _categorieRepository;
        private readonly INotificationContext _notificationContext;

        public CategorieController(ICategorieRepository categorieRepository, INotificationContext notificationContext) : base (notificationContext)
        {
            _categorieRepository = categorieRepository;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCategorie(Categorie categorie)
        {
            if (categorie != null)
                await _categorieRepository.CreateCategory(categorie);

            return Response(categorie);
        }


        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteCategorie(int id)
        {
           
            await _categorieRepository.DeleteCategory(id);
            
            return Response();
            
        }
    }
}
