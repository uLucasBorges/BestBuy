using Newtonsoft.Json.Linq;

namespace WebApiBestBuy.Domain.ViewModel
{
    public class Token
    {
        public bool Authenticated { get; set; }
        public string TokenJWT { get; set; }
        public DateTime Expiration { get; set; }
        public  string Message { get; set; }
    }
}
