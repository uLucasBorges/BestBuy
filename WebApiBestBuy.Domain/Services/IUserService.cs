using WebApiBestBuy.Domain.Models;
using WebApiBestBuy.Domain.ViewModel;

namespace WebApiBestBuy.Domain.Services;

public interface IUserService
{
    public Task<Register> CreateAccount(UserAccount user);
    public Task<ResultViewModel> LoginAccount(UserAccount user);

    
}
