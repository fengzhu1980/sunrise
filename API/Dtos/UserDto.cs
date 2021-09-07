namespace API.Dtos
{
    public class UserDto
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public double ExpiresIn { get; set; }
    }
}