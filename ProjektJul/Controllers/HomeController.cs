using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt.Data.Persistence;
using Projekt.Web.ViewModels;

namespace Projekt.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }


        public async Task<IActionResult> Index()
        {
            var vm = new IndexVm
            {
                FeaturedCvs = await GetFeaturedProfilesAsync(),
                LatestProject = await GetLatestProjectAsync()
            };

            return View(vm);
        }

        //Metod för att hämta ut profiler från databasen
        private async Task<List<CvModel>> GetFeaturedProfilesAsync()
        {
            // Hämta data från databasen
            var users = await _db.Users
                .Include(u => u.Erfarenheter)
                .Include(u => u.Skills)
                .Where(u => !u.IsPrivate)
                .OrderByDescending(u => u.FullName)
                .Take(6)
                .ToListAsync();

            // Mappa till CvModel för att få fram infon i korten på förstasidan.
            return users.Select(u => new CvModel
            {
                FullName = u.FullName ?? "Okänd",
                Title = u.Erfarenheter.FirstOrDefault()?.Position ?? "Ingen titel",
                Summary = u.Skills.Any()
                    ? string.Join(", ", u.Skills.Select(s => s.Name))
                    : "Inga färdigheter angivna",
                ProfileUrl = Url.Action("Details", "Cv", new { id = u.Id }) ?? $"/Cv/Details/{u.Id}",
                AvatarUrl = string.IsNullOrEmpty(u.ProfileImagePath)
                ? "/images/default-profile.png.jpg": u.ProfileImagePath,
            }).ToList();
        }

        //Metod för att hämta det senaste projektet från databasen
        private async Task<ProjektModel?> GetLatestProjectAsync()
        {
            var projectEntity = await _db.Projekts
                .Include(p => p.User)
                .OrderByDescending(p => p.Id)
                .FirstOrDefaultAsync();

            if (projectEntity == null) return null;

            // Mappa till ProjektModel för att få fram infon i projektkortet på förstasidan.
            return new ProjektModel
            {
                Id = projectEntity.Id,
                Title = projectEntity.Title,
                Summary = projectEntity.Description.Length > 500
                    ? projectEntity.Description.Substring(0, 500) + "..."
                    : projectEntity.Description,
                PublishedAt = DateTime.Now, 

            };
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
