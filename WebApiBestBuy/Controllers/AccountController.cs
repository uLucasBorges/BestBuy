﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApiBestBuy.Api.Controllers;
using WebApiBestBuy.Domain.Notifications;
using WebApiBestBuy.Domain.Services;

namespace WebApiBestBuy.Controllers
{
    [Route("[Controller]")]
    public class AccountController : BaseController
    {
        private readonly IUserService _userService;

        public AccountController(INotificationContext notificationContext, IUserService userService) : base(notificationContext)
        {
            _userService = userService;
        }

        [HttpPost("Registry")]
        public async Task<IActionResult> CreateAccount(IdentityUser user)
        {
             var result = await _userService.CreateAccount(user);
            return Response(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAccount(IdentityUser user)
        {
            var result = await _userService.LoginAccount(user);
            return Response(result);
        }
    }
}
