using System.ComponentModel.DataAnnotations;

namespace Web.Areas.Admin.Models
{
    public class CreateRoleVM
    {
        [Required(ErrorMessage = "Bu Alan Zorunludur!...")]
        [MinLength(3, ErrorMessage = "En Az 3 Karakter Girmelisiniz!...")]
        [MaxLength(120, ErrorMessage = "120 Karakter Sınırını Geçtiniz!...")]
        public required string Name { get; set; }
    }
}
