



namespace WebApiBestBuy.Domain.Models
{
    public class UserAccount 
    {
        public string Email { get; set; }
        public string Password { get; set; }

        //[Compare(nameof(Password), ErrorMessage = "Password mismatch")]
        //public string ConfirmPassword { get; set; }
    }
}
