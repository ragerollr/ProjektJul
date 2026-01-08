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
        public DbSet<Projects> Projekts { get; set; } = default!;

        // Rättstavat: Utbildningar (inte Utildningar)
        public DbSet<Utbildning> Utbildningar { get; set; } = default!;

        // Behåll Messages för Inbox + Notifikationer
        public DbSet<Message> Messages => Set<Message>();
    }
}
