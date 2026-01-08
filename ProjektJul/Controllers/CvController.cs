using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt.Data.Models;
using Projekt.Data.Persistence;
using Projekt.Web.ViewModels;
using System.Security.Claims;

namespace Projekt.Web.Controllers
{
    public class CvController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CvController(ApplicationDbContext db)
        {
            _db = db;
        }

        // =========================
        // KRAV 4/6: Min egen profil
        // =========================
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> MyProfile()
        {
            var myId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(myId))
                return RedirectToAction("Login", "Account");

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == myId);
            if (user == null)
                return RedirectToAction("Login", "Account");

            // OBS: Er data verkar inte vara kopplad till userId ännu i era tabeller.
            // Därför visar vi "allt" just nu (som i din kompis kod), tills ni kopplar per user.
            var skillsFromDb = await _db.Skills.ToListAsync();
            var utbildningarFromDb = await _db.Utbildningar.ToListAsync();
            var erfarenheterFromDb = await _db.Erfarenheter.ToListAsync();

            var vm = new CvViewModel
            {
                UserId = user.Id,
                FullName = user.FullName ?? user.Email ?? "Okänd",
                Email = user.Email ?? "",
                Address = "",
                IsPublic = !user.IsPrivate,
                ProfileImageUrl = "/images/default-profile.png",

                // Dessa properties måste matcha CvViewModel ni använder i projektet:
                Skills = skillsFromDb,
                Utbildningar = utbildningarFromDb,
                Erfarenheter = erfarenheterFromDb,

                Projects = new() { "CV-portal", "API-projekt" }
            };

            // Säkert oavsett mapp-case (CV vs Cv)
            return View("~/Views/CV/MyProfile.cshtml", vm);
        }

        // ============================================
        // KRAV 5/6/7 (+ för meddelanden / details-sida)
        // ============================================
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return NotFound();

            // Krav 6: privat profil syns bara för inloggade
            var isLoggedIn = User.Identity?.IsAuthenticated ?? false;
            if (user.IsPrivate && !isLoggedIn)
            {
                var returnUrl = Url.Action("Details", "Cv", new { id });
                return RedirectToAction("Login", "Account", new { returnUrl });
            }

            var skillsFromDb = await _db.Skills.ToListAsync();
            var utbildningarFromDb = await _db.Utbildningar.ToListAsync();
            var erfarenheterFromDb = await _db.Erfarenheter.ToListAsync();

            var vm = new CvViewModel
            {
                UserId = user.Id,
                FullName = user.FullName ?? user.Email ?? "Okänd",
                Email = user.Email ?? "",
                Address = "",
                IsPublic = !user.IsPrivate,
                ProfileImageUrl = "/images/default-profile.png",

                Skills = skillsFromDb,
                Utbildningar = utbildningarFromDb,
                Erfarenheter = erfarenheterFromDb,

                Projects = new() { "CV-portal", "API-projekt" }
            };

            return View("~/Views/CV/MyProfile.cshtml", vm);
        }

        // =========================
        // (Om ni har Edit-sidan kvar)
        // =========================
        [Authorize]
        [HttpGet]
        public IActionResult Edit()
        {
            // Om ni redan har en Edit.cshtml som bygger på EditCvViewModel:
            // Behåll enkel default så länge.
            var vm = new EditCvViewModel
            {
                FirstName = "",
                LastName = "",
                Address = "",
                IsPublic = true,
                Skills = "",
                Education = "",
                Experience = "",
                Projects = new()
            };

            return View(vm);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditCvViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // När ni kopplar riktig profil-data: spara här.
            return RedirectToAction(nameof(MyProfile));
        }

        // =========================
        // -------- LÄGG TILL -------
        // =========================
        [Authorize]
        [HttpGet]
        public IActionResult AddEducation()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEducation(CreateEducationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var utbildning = new Utbildning
            {
                SchoolName = model.SchoolName,
                Degree = model.Degree,
                StartDate = model.StartDate,
                EndDate = model.EndDate
            };

            _db.Utbildningar.Add(utbildning);
            _db.SaveChanges();

            return RedirectToAction(nameof(Edit));
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddErfarenhet()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddErfarenhet(CreateErfarenhetViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var erfarenhet = new Erfarenhet
            {
                CompanyName = model.CompanyName,
                Position = model.Position,
                Description = model.Description,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                UserId = null // kopplas senare när ni binder till inloggad användare
            };

            _db.Erfarenheter.Add(erfarenhet);
            _db.SaveChanges();

            return RedirectToAction(nameof(Edit));
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddSkill()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddSkill(CreateSkillViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var skill = new Skill
            {
                Name = model.Name,
                Level = model.Level
            };

            _db.Skills.Add(skill);
            _db.SaveChanges();

            return RedirectToAction(nameof(Edit));
        }

        // =========================
        // -------- RADERA ----------
        // =========================
        [Authorize]
        [HttpGet]
        public IActionResult DeleteUtbildningConfirm(int id)
        {
            var utbildning = _db.Utbildningar.Find(id);
            if (utbildning == null) return NotFound();

            return View(utbildning);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteUtbildningConfirmed(int id)
        {
            var utbildning = _db.Utbildningar.Find(id);
            if (utbildning != null)
            {
                _db.Utbildningar.Remove(utbildning);
                _db.SaveChanges();
            }

            return RedirectToAction(nameof(Edit));
        }

        [Authorize]
        [HttpGet]
        public IActionResult DeleteErfarenhetConfirm(int id)
        {
            var erfarenhet = _db.Erfarenheter.Find(id);
            if (erfarenhet == null) return NotFound();

            return View(erfarenhet);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteErfarenhetConfirmed(int id)
        {
            var erfarenhet = _db.Erfarenheter.Find(id);
            if (erfarenhet != null)
            {
                _db.Erfarenheter.Remove(erfarenhet);
                _db.SaveChanges();
            }

            return RedirectToAction(nameof(Edit));
        }
    }
}
