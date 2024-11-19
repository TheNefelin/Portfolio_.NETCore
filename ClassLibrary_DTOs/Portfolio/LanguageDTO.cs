using System.ComponentModel.DataAnnotations;

namespace ClassLibrary_DTOs.Portfolio
{
    public class LanguageDTO
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        public string ImgUrl { get; set; } = string.Empty;
    }
}
