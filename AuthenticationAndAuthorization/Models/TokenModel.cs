namespace AuthenticationAndAuthorization.Models
{
    public class TokenModel
    {
        public string AcessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
