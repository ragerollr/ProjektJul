using Microsoft.AspNetCore.Identity;

namespace Projekt.Data.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public bool IsPrivate { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
