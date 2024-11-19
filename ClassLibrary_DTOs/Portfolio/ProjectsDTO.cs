namespace ClassLibrary_DTOs.Portfolio
{
    public class ProjectsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImgUrl { get; set; } = string.Empty;
        public List<LanguageDTO> Languages { get; set; } = new List<LanguageDTO>();
        public List<TechnologyDTO> Technologies { get; set; } = new List<TechnologyDTO>();
    }
}
