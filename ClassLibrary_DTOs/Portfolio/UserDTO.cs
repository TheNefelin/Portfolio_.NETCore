namespace ClassLibrary_DTOs.Portfolio
{
    public class UserDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Hash1 { get; set; } = string.Empty;
        public string Salt1 { get; set; } = string.Empty;
        public string Hash2 { get; set; } = string.Empty;
        public string Salt2 { get; set; } = string.Empty;
        public string SqlToken { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
