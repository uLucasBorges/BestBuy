
using Microsoft.AspNetCore.Identity;

namespace WebApiBestBuy.Domain.Models
{
    public class UserAccount  
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        //[Compare(nameof(Password), ErrorMessage = "Password mismatch")]
        //public string ConfirmPassword { get; set; }
    }
}
