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
            var skillsFromDb = _context.Skills.ToList();

            var utbildningarFromDb = _context.Utbildningar.ToList();

            var erfarenheterFromDb = _context.Erfarenheter.ToList();

            var vm = new CvViewModel
            {
                FullName = "Elin Sundell",
                Email = "elin@test.se",
                Address = "Östersund",
                IsPublic = true,

                ProfileImageUrl = "/images/default-profile.png",

                Skills = skillsFromDb,
                Utbildningar = utbildningarFromDb,
                Erfarenheter = erfarenheterFromDb,
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

                Skills = _context.Skills.ToList(),
                Utbildningar = _context.Utbildningar.ToList(),
                Erfarenheter = _context.Erfarenheter.ToList(),

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




        //----------------LÄGG TILL---------------------


        [HttpGet]
        public IActionResult AddEducation()
        {
            return View();
        }

      

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEducation(CreateEducationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var utbildning = new Utbildning
            {
                SchoolName = model.SchoolName,
                Degree = model.Degree,
                StartDate = model.StartDate,
                EndDate = model.EndDate
            };

            _context.Utbildningar.Add(utbildning);
            _context.SaveChanges();

            return RedirectToAction("Edit");
        }

        [HttpGet]
        public IActionResult AddErfarenhet()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddErfarenhet(CreateErfarenhetViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var erfarenhet = new Erfarenhet
            {
                CompanyName = model.CompanyName,
                Position = model.Position,
                Description = model.Description,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                UserId = null // kopplas senare
            };

            _context.Erfarenheter.Add(erfarenhet);
            _context.SaveChanges();

            return RedirectToAction("Edit");
        }

        [HttpGet]
        public IActionResult AddSkill()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddSkill(CreateSkillViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var skill = new Skill
            {
                Name = model.Name,
                Level = model.Level
            };

            _context.Skills.Add(skill);
            _context.SaveChanges();

            return RedirectToAction("Edit");
        }


        //-------------RADERA--------------------

        [HttpGet]
        public IActionResult DeleteUtbildningConfirm(int id)
        {
            var utbildning = _context.Utbildningar.Find(id);

            if (utbildning == null)
            {
                return NotFound();
            }

            return View(utbildning);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteUtbildningConfirmed(int id)
        {
            var utbildning = _context.Utbildningar.Find(id);

            if (utbildning != null)
            {
                _context.Utbildningar.Remove(utbildning);
                _context.SaveChanges();
            }

            return RedirectToAction("Edit");
        }


        [HttpGet]
        public IActionResult DeleteErfarenhetConfirm(int id)
        {
            var erfarenhet = _context.Erfarenheter.Find(id);

            if (erfarenhet == null)
            {
                return NotFound();
            }

            return View(erfarenhet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteErfarenhetConfirmed(int id)
        {
            var erfarenhet = _context.Erfarenheter.Find(id);

            if (erfarenhet != null)
            {
                _context.Erfarenheter.Remove(erfarenhet);
                _context.SaveChanges();
            }

            return RedirectToAction("Edit");
        }


    }
}
