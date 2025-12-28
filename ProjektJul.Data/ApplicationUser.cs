using Microsoft.AspNetCore.Identity;

namespace ProjektJul.Data // Se till att namnet stämmer med ditt projekt
{
    // Vi ärver från IdentityUser för att få inloggning "gratis"
    public class ApplicationUser : IdentityUser
    {
        // Här lägger vi till dina egna fält enligt kravspecen
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string? Address { get; set; } // ? betyder att den får vara tom

        public string? ProfileImagePath { get; set; } // Sökväg till bilden

        public bool IsPrivate { get; set; } = false; // Offentlig som standard

        // För VG-kravet (vi lägger in det direkt så det är klart):
        public bool IsActive { get; set; } = true;
        public int ProfileVisits { get; set; } = 0;

        // Vi kommer lägga till kopplingar till CV och Projekt här senare
        // t.ex: public virtual ICollection<Skill> Skills { get; set; }
    }
}