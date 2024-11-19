namespace ClassLibrary_DTOs.Portfolio
{
    public class UrlsGrpsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<UrlDTO> Urls { get; set; } = new();
    }
}
