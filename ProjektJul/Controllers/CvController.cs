using Microsoft.AspNetCore.Mvc;
using Projekt.Web.ViewModels;

namespace Projekt.Web.Controllers
{
    public class CvController : Controller
    {
        public IActionResult Index()
        {

            var vm = new CvViewModel
            {
                FullName = "Elin Sundell",
                Email = "elin@test.se",
                Address = "Östersund",
                IsPublic = true,
                ProfileImageUrl = "/images/default-profile.png",

                Skills = new() { "C#", "ASP.NET", "SQL" },
                Educations = new() { "Uppsala universitet" },
                Experiences = new() { "Praktik – Backend" },
                Projects = new() { "CV-portal", "API-projekt" }
            };

                return View(vm);


        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }
    }
}
