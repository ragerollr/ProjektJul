using Projekt.Data.Models;

namespace Projekt.Web.ViewModels
{
    public class CvViewModel
    {
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Address { get; set; } = "";

        public bool IsPublic { get; set; }
        public string ProfileImageUrl { get; set; } = "";

        public List<Skill> Skills { get; set; } = new();
        public List<Utbildning> Utbildningar { get; set; } = new();
        public List<Erfarenhet> Erfarenheter { get; set; } = new();

        public List<string> Projects { get; set; } = new();
        public string UserId { get; set; } = "";
    }
}
