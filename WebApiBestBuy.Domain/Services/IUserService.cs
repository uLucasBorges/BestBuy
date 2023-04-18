﻿using Microsoft.AspNet.Identity.EntityFramework;
using WebApiBestBuy.Domain.ViewModel;

namespace WebApiBestBuy.Domain.Services;

public interface IUserService
{
    public Task<ResultViewModel> CreateAccount(IdentityUser user);
    public Task<ResultViewModel> LoginAccount(IdentityUser user);

    
}
