using System.ComponentModel.DataAnnotations;

namespace Projekt.Web.ViewModels
{
    public class ChangePasswordVm
    {
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = "";

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = "";

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Lösenorden matchar inte")]
        public string ConfirmNewPassword { get; set; } = "";
    }
}
