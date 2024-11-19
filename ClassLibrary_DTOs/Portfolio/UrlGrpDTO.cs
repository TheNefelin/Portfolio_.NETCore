using System.ComponentModel.DataAnnotations;

namespace ClassLibrary_DTOs.Portfolio
{
    public class UrlGrpDTO
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public bool Status { get; set; }
    }
}
