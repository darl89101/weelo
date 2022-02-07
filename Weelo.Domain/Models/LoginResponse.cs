namespace Weelo.Domain.Models
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }
        public DateTime ExpiresIn { get; set; }
    }
}
