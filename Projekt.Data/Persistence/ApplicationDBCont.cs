using CVSite.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Projekt.Data.Identity;

namespace Projekt.Data.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Erfarenhet> Erfarenhets { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Projects> Projekts { get; set; }
        public DbSet<Utbildning> Utbildningar { get; set; }
    }
}
