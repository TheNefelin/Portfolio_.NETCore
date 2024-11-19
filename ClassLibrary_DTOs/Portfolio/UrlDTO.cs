using System.ComponentModel.DataAnnotations;

namespace ClassLibrary_DTOs.Portfolio
{
    public class UrlDTO
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MaxLength(256)]
        public string Url { get; set; } = string.Empty;
        [Required]
        public bool Status { get; set; }
        [Required]
        public int Id_UrlGrp { get; set; }
    }
}
