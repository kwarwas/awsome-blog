namespace AwesomeBlog.Api.Settings
{
    public class JwtSettings
    {
        public string ValidIssuer { get; set; } = "https://localhost:5001";
        public string ValidAudience { get; set; } = "https://localhost:5001";
        public string Secret { get; set; } = "12A5379A-5F05-41A5-98A4-AE3B911AEBFE";
        public int LifetimeInSeconds { get; set; } = 3600;
    }
}