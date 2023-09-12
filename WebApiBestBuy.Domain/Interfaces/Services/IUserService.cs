using WebApiBestBuy.Domain.Models;
using WebApiBestBuy.Domain.ViewModel;

namespace WebApiBestBuy.Domain.Interfaces.Services;

public interface IUserService
{
    public Task<Register> CreateAccount(UserAccount user);
    public Task<ResultViewModel> LoginAccount(UserAccount user);


}
