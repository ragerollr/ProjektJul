using System.ComponentModel.DataAnnotations;

namespace Projekt.Web.ViewModels
{
    public class RegisterVm
    {
        [Required, StringLength(80)]
        public string FullName { get; set; } = "";

        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = "";

        [Required, DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Lösenorden matchar inte.")]
        public string ConfirmPassword { get; set; } = "";
    }
}
