using WebApiBestBuy.Domain.Notifications;
using Microsoft.AspNetCore.Mvc;
using WebApiBestBuy.Domain.Interfaces;
using WebApiBestBuy.Domain.Models;
using AutoMapper;
using WebApiBestBuy.Domain.ViewModel;
using System.Text.Unicode;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace WebApiBestBuy.Api.Controllers
{
    [Route("[Controller]")]
    public class CategorieController : BaseController
    {
        private readonly ICategorieRepository _categorieRepository;
        private readonly INotificationContext _notificationContext;
        private readonly IMapper _mapper;
        public CategorieController(ICategorieRepository categorieRepository, IMapper mapper,INotificationContext notificationContext) : base (notificationContext)
        {
            _categorieRepository = categorieRepository;
            _mapper = mapper;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCategorie(CategorieViewModel categorie)
        {
            var maped = _mapper.Map<Categorie>(categorie);

            if (!maped.IsValid)
                return BadRequest(maped.Erros);



            await _categorieRepository.CreateCategory(maped);

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
