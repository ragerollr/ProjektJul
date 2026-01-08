using Microsoft.AspNetCore.Identity;

namespace Projekt.Data.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public bool IsPrivate { get; set; }

    }
}
