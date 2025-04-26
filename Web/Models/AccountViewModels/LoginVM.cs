using System.ComponentModel.DataAnnotations;

namespace Web.Models.AccountViewModels
{
    public class LoginVM
    {
        [Display(Name = "Kullanıcı Adı")]
        [Required(ErrorMessage = "Bu Alan Zorunludur!...")]
        [MaxLength(30, ErrorMessage = "30 Karakter Sınırını Aştınız!...")]
        [MinLength(3, ErrorMessage = "En Az 3 Karakter Girmelisiniz!...")]
        [RegularExpression("^[a-zA-Z0-9 ıİşŞğĞöÖüÜ-]+$", ErrorMessage = "Sadece Harf, Sayı, Boşluk ve \"-\" Girebilirsiniz!...")]
        public string UserName { get; set; }

        [Display(Name = "Parola")]
        [Required(ErrorMessage = "Bu Alan Zorunludur!...")]
        [MaxLength(10, ErrorMessage = "10 Karakter Sınırını Aştınız!...")]
        [MinLength(3, ErrorMessage = "En Az 3 Karakter Girmelisiniz!...")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
