using Projekt.Data.Models;
using Microsoft.AspNetCore.Identity;
using Projekt.Data.Models;

namespace Projekt.Data.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public bool IsPrivate { get; set; }
        public virtual ICollection<Erfarenhet> Erfarenheter { get; set; } = new List<Erfarenhet>();
        public virtual ICollection<Utbildning> Utbildningar { get; set; } = new List<Utbildning>();
        public virtual ICollection<Skill> Skills { get; set; } = new List<Skill>();
        public virtual ICollection<Projects> OwnedProjects { get; set; } = new List<Projects>();
        public virtual ICollection<Projects> CollaboratingProjects { get; set; } = new List<Projects>();

        public string? ProfileImagePath { get; set; }


    }
}
