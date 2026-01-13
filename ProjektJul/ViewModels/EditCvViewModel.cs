using Projekt.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Projekt.Web.ViewModels
{
    public class EditCvViewModel
    {

        [Required(ErrorMessage = "Förnamn måste anges")]
        [StringLength(50)]
        public string FirstName { get; set; } = "";

        [Required(ErrorMessage = "Efternamn måste anges")]
        [StringLength(50)]
        public string LastName { get; set; } = "";

        [Required(ErrorMessage = "Adress måste anges")]
        [StringLength(100)]
        public string Address { get; set; } = "";

        [Required(ErrorMessage = "Email måste anges")]
        [EmailAddress(ErrorMessage = "Ogiltig emailadress")]
        public string Email { get; set; } = "";

        public bool IsPublic { get; set; }

        public IFormFile? ProfileImage { get; set; }

        public List<ProjectCheckBoxViewModel> Projects { get; set; } = new();

        public List<Utbildning> Utbildningar { get; set; } = new();
        public List<Erfarenhet> Erfarenheter { get; set; } = new();
        public List<Skill> Skills { get; set; } = new();
    }
}
