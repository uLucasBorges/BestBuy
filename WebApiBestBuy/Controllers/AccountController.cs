using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApiBestBuy.Api.Controllers;
using WebApiBestBuy.Domain.Interfaces.Services;
using WebApiBestBuy.Domain.Models;
using WebApiBestBuy.Domain.Notifications;

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
        public async Task<IActionResult> CreateAccount(UserAccount user)
        {
             var result = await _userService.CreateAccount(user);
            return Response(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAccount(UserAccount user)
        {
            var result = await _userService.LoginAccount(user);
            return Response(result);
        }
    }
}
