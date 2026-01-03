namespace Projekt.Web.ViewModels
{
    public class CvViewModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public bool IsPublic { get; set; }
        public string ProfileImageUrl { get; set; }

        public List<string> Skills { get; set; }
        public List<string> Educations { get; set; }
        public List<string> Experiences { get; set; }

        public List<string> Projects { get; set; }
    }
}
