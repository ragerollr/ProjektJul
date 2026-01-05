using CVSite.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt.Data.Persistence;
using Projekt.Web.ViewModels;

using System.Linq;
using static Projekt.Web.ViewModels.ProjectCheckBoxViewModel;

namespace Projekt.Web.Controllers
{
    public class CvController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CvController(ApplicationDbContext context)
        {
            _context = context;
        }



        public IActionResult MyProfile()
        {
            var skillsFromDb = _context.Skills
            .Select(s => s.Name)
            .ToList();

            var vm = new CvViewModel
            {
                FullName = "Elin Sundell",
                Email = "elin@test.se",
                Address = "Östersund",
                IsPublic = true,

                ProfileImageUrl = "/images/default-profile.png",

                Skills = skillsFromDb,
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


            return RedirectToAction("MyProfile");
        }
    

    public IActionResult TestDb()
        {
            var skill = new Skill
            {
                Name = "ASP.NET",
             
            };

            _context.Skills.Add(skill);
            int affectedRows = _context.SaveChanges();

            return Content("Skill sparad!");
        }

        public IActionResult TestUtbildning()
        {
            var utbildning = new Utbildning
            {
                SchoolName = "Örebro Universitet",
                Degree = "Systemvetenskap",
                StartDate = new DateTime(2022, 8, 15),
                EndDate = null
            };

            _context.Utbildningar.Add(utbildning);
            _context.SaveChanges();

            return RedirectToAction("MyProfile");
        }


    }
}
