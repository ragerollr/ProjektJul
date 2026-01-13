using System.ComponentModel.DataAnnotations;

namespace Projekt.Web.ViewModels
{
    public class ChangePasswordVm
    {
        [Required(ErrorMessage = "Nuvarande lösenord krävs")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = "";

        [Required(ErrorMessage = "Nytt lösenord krävs")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Lösenordet måste vara minst 6 tecken")]
        public string NewPassword { get; set; } = "";

        [Required(ErrorMessage = "Bekräfta det nya lösenordet")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Lösenorden matchar inte")]
        public string ConfirmNewPassword { get; set; } = "";
    }
}
