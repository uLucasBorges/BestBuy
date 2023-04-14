using Microsoft.AspNetCore.Identity;
using WebApiBestBuy.ViewModel;

namespace WebApiBestBuy.Services
{
    public interface IUserService
    {
        public Task<ResultViewModel> CreateAccount(IdentityUser user);
        public Task<ResultViewModel> LoginAccount(IdentityUser user);

    }
}
