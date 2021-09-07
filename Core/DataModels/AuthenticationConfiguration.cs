namespace Core.DataModels.Models
{
    public class AuthenticationConfiguration
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double ExpirationMinutes { get; set; }
        public string RefreshTokenSecret { get; set; }
        public double RefreshTokenExpirationMinutes { get; set; }
    }
}