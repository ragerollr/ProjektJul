using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Projekt.Web.ViewModels;

namespace Projekt.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // placeholder data, här kommer vi hämta från databasen efter elin och johan har löst profil, registrering och inloggning.
            var vm = new IndexVm
            {
                FeaturedCvs =
                [
                    new CvModel
                    {
                        FullName = "Johan Erlandsson",
                        Title = "Backendutvecklare",
                        Summary = "Bygger API:er i .NET och optimerar SQL",
                        ProfileUrl = "/Home/MyProfile/johan",
                        AvatarUrl = "https://via.placeholder.com/120"
                    },
                    new CvModel
                    {
                        FullName = "Elin Sundell",
                        Title = "UX/UI-designer",
                        Summary = "Designar tydliga flöden och pixelperfekta gränssnitt",
                        ProfileUrl = "/Home/MyProfile/elin",
                        AvatarUrl = "https://via.placeholder.com/120"
                    },
                    new CvModel
                    {
                        FullName = "Felix Mobäck",
                        Title = "Fullstack-utvecklare",
                        Summary = "Levererar end-to-end i React och ASP.NET Core",
                        ProfileUrl = "/Home/MyProfile/felix",
                        AvatarUrl = "https://via.placeholder.com/120"
                    },
                    new CvModel
                    {
                        FullName = "Caroline Hultman",
                        Title = "Fullstack-utvecklare",
                        Summary = "Levererar end-to-end i React och ASP.NET Core",
                        ProfileUrl = "/Home/MyProfile/caroline",
                        AvatarUrl = "https://via.placeholder.com/120"
                    },
                    new CvModel
                    {
                        FullName = "Dennis Perä",
                        Title = "Fullstack-utvecklare",
                        Summary = "Levererar end-to-end i React och ASP.NET Core",
                        ProfileUrl = "/Home/MyProfile/dennis",
                        AvatarUrl = "https://via.placeholder.com/120"
                    }
                ],
                LatestProject = new ProjektModel
                {
                    Id = 1,
                    Title = "Kundportal för logistik",
                    Summary = "Ett program för att hantera transporter och spåra leveranser i realtid. av Johan Erlandsson och Elin Sundell.",
                    PublishedAt = DateTime.UtcNow.AddDays(-3),
                    DetailUrl = "/project/details/1",
                    ImageUrl = "https://via.placeholder.com/800x400"
                }
            };

            return View(vm);
        }

        public IActionResult MyProfile()
        {
            return View();
        }

        public IActionResult Inbox()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
