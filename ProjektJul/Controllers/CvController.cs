using Microsoft.AspNetCore.Mvc;
using Projekt.Web.ViewModels;
using System.Linq;
using static Projekt.Web.ViewModels.ProjectCheckBoxViewModel;

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
            var vm = new EditCvViewModel
            {
                FirstName = "Elin",
                LastName = "Sundell",
                Address = "Östersund",
                IsPublic = true,
                Skills = "C#, ASP.NET",
                Education = "Uppsala universitet",
                Experience = "Praktik inom backend",


                 Projects = new List<ProjectCheckboxViewModel>
        {
            new() { ProjectId = 1, Title = "CV-portal", IsSelected = true },
            new() { ProjectId = 2, Title = "API-projekt", IsSelected = false },
            new() { ProjectId = 3, Title = "Webbshop", IsSelected = true }
        }

            };
            return View(vm);
        }

       [HttpPost]
       public IActionResult Edit(EditCvViewModel model)
        {
            var selectedProjects = model.Projects
            .Where(p => p.IsSelected)
            .ToList();


            return RedirectToAction("Index");
        }
    }
}
