using System.ComponentModel.DataAnnotations;

namespace Web.Models.AccountViewModels
{
    public class RegisterVM
    {
        [Display(Name = "Ad")]
        [Required(ErrorMessage = "Bu Alan Zorunludur!...")]
        [MaxLength(120, ErrorMessage = "120 Karakter Sınırını Aştınız!...")]
        [MinLength(2, ErrorMessage = "En Az 2 Karakter Girmelisiniz!...")]
        [RegularExpression("^[a-zA-ZıİşŞğĞöÖüÜ ]+$", ErrorMessage = "Sadece Harf ve Boşluk Girebilirsiniz!...")]
        public string FirstName { get; set; }

        [Display(Name = "Soyad")]
        [Required(ErrorMessage = "Bu Alan Zorunludur!...")]
        [MaxLength(120, ErrorMessage = "120 Karakter Sınırını Aştınız!...")]
        [MinLength(2, ErrorMessage = "En Az 2 Karakter Girmelisiniz!...")]
        [RegularExpression("^[a-zA-ZıİşŞğĞöÖüÜ ]+$", ErrorMessage = "Sadece Harf ve Boşluk Girebilirsiniz!...")]
        public string LastName { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        [Required(ErrorMessage = "Bu Alan Zorunludur!...")]
        [MaxLength(30, ErrorMessage = "30 Karakter Sınırını Aştınız!...")]
        [MinLength(3, ErrorMessage = "En Az 3 Karakter Girmelisiniz!...")]
        [RegularExpression("^[a-zA-Z0-9 ıİşŞğĞöÖüÜ-]+$", ErrorMessage = "Sadece Harf, Sayı, Boşluk ve \"-\" Girebilirsiniz!...")]
        public string UserName { get; set; }

        [Display(Name = "E-Posta")]
        [EmailAddress(ErrorMessage = "E-Posta Formatında Giriş Yapınız!...")]
        [Required(ErrorMessage = "Bu Alan Zorunludur!...")]
        public string Email { get; set; }

        [Display(Name = "Doğum Tarihi")]
        [Required(ErrorMessage = "Bu Alan Zorunludur!...")]
        [DataType(DataType.Date)]
        public DateOnly Birthdate { get; set; }

        [Display(Name = "Parola")]
        [Required(ErrorMessage = "Bu Alan Zorunludur!...")]
        [MaxLength(10, ErrorMessage = "10 Karakter Sınırını Aştınız!...")]
        [MinLength(3, ErrorMessage = "En Az 3 Karakter Girmelisiniz!...")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Parola Tekrar")]
        [Required(ErrorMessage = "Bu Alan Zorunludur!...")]
        [MaxLength(10, ErrorMessage = "10 Karakter Sınırını Aştınız!...")]
        [MinLength(3, ErrorMessage = "En Az 3 Karakter Girmelisiniz!...")]
        [Compare(nameof(Password), ErrorMessage = "Şifreler Uyuşmuyor!...")]
        [DataType(DataType.Password)]
        public string CheckPassword { get; set; }
    }
}
