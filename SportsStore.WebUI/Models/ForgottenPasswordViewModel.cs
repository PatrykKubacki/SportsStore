using System.ComponentModel.DataAnnotations;

namespace SportsStore.WebUI.Models
{
    public class ForgottenPasswordViewModel
    {
        [Required(ErrorMessage = "Podaj adres e-mail")]
        [Display(Name = "Adres e-mail")]
        [EmailAddress(ErrorMessage = "Niepoprawny adres e - mail")]
        [StringLength(50, ErrorMessage = "Maksymalnie 50 znaków ")]
        public string Email { get; set; }
    }

}
