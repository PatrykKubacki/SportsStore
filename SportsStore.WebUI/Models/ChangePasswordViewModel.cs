using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SportsStore.WebUI.Models
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Podaj aktualne hasło")]
        [Display(Name = "Aktualne hasło")]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "Maksymalnie 50 znaków ")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Podaj nowe hasło")]
        [Display(Name = "Nowe hasło")]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "Maksymalnie 50 znaków ")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Potwierdz hasło")]
        [Display(Name = "Potwierdz nowe hasło")]
        [Compare("NewPassword",ErrorMessage = "Podałeś inne hasło")]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "Maksymalnie 50 znaków ")]
        public string ConfirmedPassword { get; set; }
    }
}