using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiBestBuy.Domain.Notifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using WebApiBestBuy.ViewModel;

namespace WebApiBestBuy.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _role;
        private readonly IConfiguration _configuration;
        private readonly INotificationContext _notificationContext;

        public UserService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> role,
            IConfiguration configuration, INotificationContext notificationContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _role = role;
            _configuration = configuration;
            _notificationContext = notificationContext;
        }

        public async Task<ResultViewModel> CreateAccount(IdentityUser user)
        {
           
            var userExists = _userManager.Users.Where(x => x.Email == user.Email).FirstOrDefault();
            if (userExists == null)
            {
                await _userManager.CreateAsync(user);

                await ValidationExistsRole();

                await _userManager.AddToRoleAsync(user, "Member");
            }
            else
                _notificationContext.AddNotification(400, "Usuário já existente");


            return new ResultViewModel
            {
                data = user,
                Success = _notificationContext.HasNotifications()
            };

                
        }

        public async Task<ResultViewModel> LoginAccount(IdentityUser user)
        {
            var result = await _signInManager.PasswordSignInAsync(user.UserName,
             user.PasswordHash, isPersistent: false, lockoutOnFailure: false);

            if (!result.Succeeded)
                _notificationContext.AddNotification(400, "A senha e/ou email não conferem.");
          

            return new ResultViewModel
            {
                data = user,
                Success = _notificationContext.HasNotifications()
            };

        }


        public async Task<Token>  GenerateToken(IdentityUser userInfo)
        {

            //define declarações do usuário

            var claims = new List<Claim> {
                 new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.UserName),
                 new Claim(JwtRegisteredClaimNames.Email, userInfo.Email),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var user = new IdentityUser
            {
                UserName = userInfo.UserName,
                Email = userInfo.Email,
                EmailConfirmed = true
            };


            var roles = await GetRoles(user);

            if (roles != null)
            {

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            //gera uma chave com base em um algoritmo simetrico
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            //gera a assinatura digital do token usando o algoritmo Hmac e a chave privada
            var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //Tempo de expiracão do token.
            var expiracao = _configuration["TokenConfiguration:ExpireHours"];
            var expiration = DateTime.UtcNow.AddHours(double.Parse(expiracao));

            // classe que representa um token JWT e gera o token
            JwtSecurityToken token = new JwtSecurityToken(
              issuer: _configuration["TokenConfiguration:Issuer"],
              audience: _configuration["TokenConfiguration:Audience"],
              claims: claims,
              expires: expiration,
              signingCredentials: credenciais);


            //retorna os dados com o token e informacoes
            return new Token
            {
                Authenticated = true,
                TokenJWT = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
                Message = "Token JWT OK"
            };
        }

        /// <summary>
        /// método para verificar se existe roles ja cadastradas, se não houver, as cria.
        /// </summary>
        /// <returns></returns>
        public async Task ValidationExistsRole()
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
}

