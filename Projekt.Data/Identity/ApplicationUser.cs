using CVSite.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Projekt.Data.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public bool IsPrivate { get; set; }
        public virtual ICollection<Erfarenhet> Erfarenheter { get; set; } = new List<Erfarenhet>();
        public virtual ICollection<Utbildning> Utbildningar { get; set; } = new List<Utbildning>();
        public virtual ICollection<Skill> Skills { get; set; } = new List<Skill>();
        public virtual ICollection<Projects> Projects { get; set; } = new List<Projects>();

    }
}
