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
            // Uni-nivå: startsidan ska inte lista privata profiler (krav 6)
            var featured = await _db.Users
                .Where(u => !u.IsPrivate)
                .OrderBy(u => u.FullName ?? u.Email)
                .Take(6)
                .Select(u => new CvModel
                {
                    FullName = u.FullName ?? u.Email ?? "Okänd",
                    Title = "Profil", // koppla senare till riktig titel
                    Summary = "Klicka för att se profil.",
                    ProfileUrl = Url.Action("Details", "Cv", new { id = u.Id }) ?? $"/Cv/Details/{u.Id}",
                    AvatarUrl = "https://via.placeholder.com/120"
                })
                .ToListAsync();

            var vm = new IndexVm
            {
                FeaturedCvs = featured,
                LatestProject = new ProjektModel
                {
                    Id = 1,
                    Title = "Kundportal för logistik",
                    Summary = "Ett program för att hantera transporter och spåra leveranser i realtid.",
                    PublishedAt = DateTime.UtcNow.AddDays(-3),
                    DetailUrl = "/project/details/1",
                    ImageUrl = "https://via.placeholder.com/800x400"
                }
            };

            return View(vm);
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
