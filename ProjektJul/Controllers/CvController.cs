using CVSite.Data.Models;
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

            var vm = new CvViewModel
            {
                UserId = user.Id,
                FullName = user.FullName ?? user.Email ?? "Okänd",
                Email = user.Email ?? "",
                Address = "", // koppla senare till riktig adress om ni vill
                IsPublic = !user.IsPrivate,
                ProfileImageUrl = "/images/default-profile.png",

                Skills = await _db.Skills.Where(s => s.UserId == myId).ToListAsync(),
                Utbildningar = await _db.Utbildningar.Where(u => u.UserId == myId).ToListAsync(),
                Erfarenheter = await _db.Erfarenheter.Where(e => e.UserId == myId).ToListAsync(),

                Projects = new() { "CV-portal", "API-projekt" } // koppla senare
            };

            return View("~/Views/CV/MyProfile.cshtml", vm);
        }

        // ============================================
        // KRAV 5/6/7: Visa en specifik användares CV
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

            var vm = new CvViewModel
            {
                UserId = user.Id,
                FullName = user.FullName ?? user.Email ?? "Okänd",
                Email = user.Email ?? "",
                Address = "",
                IsPublic = !user.IsPrivate,
                ProfileImageUrl = "/images/default-profile.png",

                Skills = await _db.Skills.Where(s => s.UserId == id).ToListAsync(),
                Utbildningar = await _db.Utbildningar.Where(u => u.UserId == id).ToListAsync(),
                Erfarenheter = await _db.Erfarenheter.Where(e => e.UserId == id).ToListAsync(),

                Projects = new() { "CV-portal", "API-projekt" }
            };

            return View("~/Views/CV/MyProfile.cshtml", vm);
        }

        // =========================
        // Edit (visar listor + profilinfo placeholder)
        // =========================
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var myId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(myId))
                return RedirectToAction("Login", "Account");

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == myId);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var vm = new EditCvViewModel
            {
                FirstName = (user.FullName ?? "").Split(' ').FirstOrDefault() ?? "",
                LastName = (user.FullName ?? "").Split(' ').Skip(1).FirstOrDefault() ?? "",
                Address = "", // koppla till riktig adress om ni har fält
                IsPublic = !user.IsPrivate,

                Skills = await _db.Skills.Where(s => s.UserId == myId).ToListAsync(),
                Utbildningar = await _db.Utbildningar.Where(u => u.UserId == myId).ToListAsync(),
                Erfarenheter = await _db.Erfarenheter.Where(e => e.UserId == myId).ToListAsync(),
                Projects = new()
            };

            return View(vm);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditCvViewModel model)
        {
            var myId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(myId))
                return RedirectToAction("Login", "Account");

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == myId);
            if (user == null)
                return RedirectToAction("Login", "Account");

            // Spara profilinfo (krav 3)
            user.FullName = $"{model.FirstName} {model.LastName}".Trim();
            user.IsPrivate = !model.IsPublic;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(MyProfile));
        }

        // =========================
        // Add Education
        // =========================
        [Authorize]
        [HttpGet]
        public IActionResult AddEducation() => View();

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEducation(CreateEducationViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var myId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(myId)) return RedirectToAction("Login", "Account");

            var utbildning = new Utbildning
            {
                SchoolName = model.SchoolName,
                Degree = model.Degree,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                UserId = myId
            };

            _db.Utbildningar.Add(utbildning);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Edit));
        }

        // =========================
        // Add Experience
        // =========================
        [Authorize]
        [HttpGet]
        public IActionResult AddErfarenhet() => View();

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddErfarenhet(CreateErfarenhetViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var myId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(myId)) return RedirectToAction("Login", "Account");

            var erfarenhet = new Erfarenhet
            {
                CompanyName = model.CompanyName,
                Position = model.Position,
                Description = model.Description,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                UserId = myId
            };

            _db.Erfarenheter.Add(erfarenhet);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Edit));
        }

        // =========================
        // Add Skill
        // =========================
        [Authorize]
        [HttpGet]
        public IActionResult AddSkill() => View();

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSkill(CreateSkillViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var myId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(myId)) return RedirectToAction("Login", "Account");

            var skill = new Skill
            {
                Name = model.Name,
                Level = model.Level,
                UserId = myId
            };

            _db.Skills.Add(skill);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Edit));
        }

        // =========================
        // Delete Education
        // =========================
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DeleteUtbildningConfirm(int id)
        {
            var myId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(myId)) return RedirectToAction("Login", "Account");

            var utbildning = await _db.Utbildningar.FirstOrDefaultAsync(u => u.Id == id);
            if (utbildning == null) return NotFound();

            if (utbildning.UserId != myId) return Forbid();

            return View(utbildning);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUtbildningConfirmed(int id)
        {
            var myId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(myId)) return RedirectToAction("Login", "Account");

            var utbildning = await _db.Utbildningar.FirstOrDefaultAsync(u => u.Id == id);
            if (utbildning == null) return NotFound();

            if (utbildning.UserId != myId) return Forbid();

            _db.Utbildningar.Remove(utbildning);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Edit));
        }

        // =========================
        // Delete Experience
        // =========================
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DeleteErfarenhetConfirm(int id)
        {
            var myId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(myId)) return RedirectToAction("Login", "Account");

            var erfarenhet = await _db.Erfarenheter.FirstOrDefaultAsync(e => e.Id == id);
            if (erfarenhet == null) return NotFound();

            if (erfarenhet.UserId != myId) return Forbid();

            return View(erfarenhet);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteErfarenhetConfirmed(int id)
        {
            var myId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(myId)) return RedirectToAction("Login", "Account");

            var erfarenhet = await _db.Erfarenheter.FirstOrDefaultAsync(e => e.Id == id);
            if (erfarenhet == null) return NotFound();

            if (erfarenhet.UserId != myId) return Forbid();

            _db.Erfarenheter.Remove(erfarenhet);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Edit));
        }
    }
}
