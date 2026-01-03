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
        public DbSet<Utbildning> Utildningar { get; set; }
    

    protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Projects>().HasData(
                new Projects
                {
                    Id = 1,
                    Title = "Projekt 1",
                    Description = "Beskrivning av projekt 1",
                    UserId = "user1"
                },
                new Projects
                {
                    Id = 2,
                    Title = "Projekt 2",
                    Description = "Beskrivning av projekt 2",
                    UserId = "user2"
                }
            );
        }
}
}
