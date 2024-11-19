using System.ComponentModel.DataAnnotations;

namespace ClassLibrary_DTOs.PasswordManager
{
    public class CoreRequestDTO<T>
    {
        [Required]
        public string Id_User { get; set; } = string.Empty;
        [Required]
        public string Sql_Token { get; set; }
        public string Password { get; set; } = string.Empty;
        public T? CoreData { get; set; } // Solo para operaciones de inserción/actualización
    }
}
