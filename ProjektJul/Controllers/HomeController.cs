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

            // Mappa till CvModel
            return users.Select(u => new CvModel
            {
                FullName = u.FullName ?? "Okänd",
                Title = u.Erfarenheter.FirstOrDefault()?.Position ?? "Ingen titel",
                Summary = u.Skills.Any()
                    ? string.Join(", ", u.Skills.Select(s => s.Name))
                    : "Inga färdigheter angivna",
                ProfileUrl = Url.Action("Details", "Cv", new { id = u.Id }) ?? $"/Cv/Details/{u.Id}",
                AvatarUrl = "https://via.placeholder.com/150"
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

            // Mappa till ProjektModel
            return new ProjektModel
            {
                Id = projectEntity.Id,
                Title = projectEntity.Title,
                Summary = projectEntity.Description.Length > 500
                    ? projectEntity.Description.Substring(0, 500) + "..."
                    : projectEntity.Description,
                PublishedAt = DateTime.Now, // Eller ditt CreatedDate om du fixade det
                DetailUrl = Url.Action("Details", "Projects", new { id = projectEntity.Id }) ?? $"/Projects/Details/{projectEntity.Id}",
                ImageUrl = "https://via.placeholder.com/800x400?text=" + Uri.EscapeDataString(projectEntity.Title)
            };
        }

        // Viktigt: om något fortfarande pekar på /Home/MyProfile, skicka vidare rätt.
        public IActionResult MyProfile()
        {
            return RedirectToAction("MyProfile", "Cv");
        }

        // Dessa behövs inte längre om du kör AccountController + MessagesController,
        // men kan ligga kvar om ni inte vill städa nu.
        public IActionResult Inbox() => RedirectToAction("Inbox", "Messages");
        public IActionResult Login() => RedirectToAction("Login", "Account");
        public IActionResult Register() => RedirectToAction("Register", "Account");

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
