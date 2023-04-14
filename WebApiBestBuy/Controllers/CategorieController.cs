﻿using BestBuy.Core.Interfaces;
using BestBuy.Core.Notification;
using Microsoft.AspNetCore.Mvc;
using WebApiBestBuy.Models;

namespace WebApiBestBuy.Controllers
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
