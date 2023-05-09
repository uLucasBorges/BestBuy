using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiBestBuy.Domain.Notifications;
using Microsoft.IdentityModel.Tokens;
using WebApiBestBuy.Domain.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using WebApiBestBuy.Domain.Models;
using System.Text.Unicode;
using System;

namespace WebApiBestBuy.Domain.Services;

public class UserService : IUserService
{
    private readonly INotificationContext _notificationContext;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly RoleManager<IdentityRole> _role;
    private readonly IMapper _mapper;

    public UserService(UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager, IConfiguration configuration, RoleManager<IdentityRole> role, IMapper mapper, INotificationContext notificationContext)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _role = role;
        _mapper = mapper;
        _notificationContext = notificationContext;

    }
    public async Task<Register> CreateAccount(UserAccount user)
    {

        var mapped = _mapper.Map<IdentityUser>(user);

        var userExists = _userManager.Users.Where(x => x.UserName == mapped.UserName).FirstOrDefault();


        if (userExists == null)
        {
            var result = await _userManager.CreateAsync(mapped, user.Password);
            if (result.Succeeded)
            {
                await ValidationExistsRole();

                await _userManager.AddToRoleAsync(mapped, "Member");
            }

            else
            {
                foreach (var erro in result.Errors)
                {
                    _notificationContext.AddNotification(400, erro.Description);
                }
            }
        }
        else
            _notificationContext.AddNotification(400, "Usuário já existente");


        return new Register
        {
            Registered = !_notificationContext.HasNotifications(),
            UserAccount = user.UserName,
            Message =  "Usuário cadastrado com sucesso."
        };

            
    }

    public async Task<ResultViewModel> LoginAccount(UserAccount user)
    {


        var userExists = _userManager.Users.Where(x => x.UserName == user.UserName).FirstOrDefault();

        if (userExists != null)
        {
            var result = await _signInManager.PasswordSignInAsync(userExists.UserName,
             user.Password, isPersistent: false, lockoutOnFailure: false); ;

            if (!result.Succeeded)

                _notificationContext.AddNotification(400, "Não foi possivel realizar o Login, tente novamente.");
        }
        else
        {
            _notificationContext.AddNotification(404, "Usuário não localizado.");

        }


        return new ResultViewModel ( await GenerateToken(userExists),  !_notificationContext.HasNotifications() );

    }


    private async Task<Token>  GenerateToken(IdentityUser userInfo)
    {

        var roles = await GetRoles(userInfo);


        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.UserName),
                new Claim(JwtRegisteredClaimNames.Email, userInfo.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                //new Claim("internalUser", userInfo.UserName.ToString(), ClaimValueTypes.Boolean),

            }),
            NotBefore = DateTime.Now,
            Expires = DateTime.Now.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        };



        if (roles != null)
        {
            foreach (var role in roles)
            {
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));
            }
        }


        var token = tokenHandler.CreateToken(tokenDescriptor);


        //retorna os dados com o token e informacoes
        return new Token
        {
            Authenticated = true,
            TokenJWT = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = (DateTime)tokenDescriptor.Expires,
            Message = "Token JWT OK"
        };
    }

    /// <summary>
    /// método para verificar se existe roles ja cadastradas, se não houver, as cria.
    /// </summary>
    /// <returns></returns>
    private async Task ValidationExistsRole()
    {
        
        var roleNames = new List<string>() { "Admin", "Member" };

        foreach (string name in roleNames)
        {
            var roleExist = await _role.RoleExistsAsync(name);
            if (!roleExist)
            {
                await _role.CreateAsync(new IdentityRole(name));
            }
        }
    }

    /// <summary>
    /// obtém as roles de determinado usuário
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<IList<string>> GetRoles(IdentityUser user)
    {
        var userr = _userManager.Users.Where(x => x.Email == user.Email).FirstOrDefault();

        IList<string>? roles = await _userManager.GetRolesAsync(userr);
        return roles;
    }
}

