using WebApiBestBuy.Domain.Notifications;
using Microsoft.AspNetCore.Mvc;
using WebApiBestBuy.Domain.Models;
using AutoMapper;
using WebApiBestBuy.Domain.ViewModel;
using System.Text.Unicode;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using WebApiBestBuy.Domain.Interfaces.Repositories;
using WebApiBestBuy.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;

namespace WebApiBestBuy.Api.Controllers
{
    [Route("[Controller]")]
    [Authorize("Admin")]
    public class CategorieController : BaseController
    {
        private readonly ICategorieService _categorieService;
        private readonly IMapper _mapper;
        public CategorieController(ICategorieService categorieService, IMapper mapper,INotificationContext notificationContext) : base (notificationContext)
        {
            _categorieService = categorieService;
            _mapper = mapper;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCategorie(CategorieViewModel categorie)
        {
            var maped = _mapper.Map<Categorie>(categorie);

            if (!maped.IsValid)
                return BadRequest(maped.Erros);


            await _categorieService.CreateCategory(maped);

            return Response(categorie);
        }


        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteCategorie(int id)
        {
           if(id != 0)
            await _categorieService.DeleteCategory(id);
            
            return Response();
            
        }
    }
}
