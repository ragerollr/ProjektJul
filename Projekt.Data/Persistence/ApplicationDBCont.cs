using CVSite.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Projekt.Data.Identity;
using Projekt.Data.Models;

namespace Projekt.Data.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Erfarenhet> Erfarenheter { get; set; } = default!;
        public DbSet<Skill> Skills { get; set; } = default!;
        public DbSet<Utbildning> Utbildningar { get; set; } = default!;
        public DbSet<Projects> Projekts { get; set; } = default!;

        // Messages (krav inbox/notifiering)
        public DbSet<Message> Messages { get; set; } = default!;
    }

}
