namespace ClassLibrary_DTOs.Auth
{
    public class JwtConfigDTO
    {
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string ExpireMin { get; set; } = string.Empty;
    }
}
