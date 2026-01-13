using Projekt.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt.Data.Persistence;
using Projekt.Web.ViewModels;
using System.Security.Claims;

namespace Projekt.Web.Controllers
{
    public class CvController : Controller
    {
        private readonly ApplicationDbContext _db;

        private readonly IWebHostEnvironment _env;


        public CvController(ApplicationDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
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
                Address = user.Address ?? "",

                IsPublic = !user.IsPrivate,
                
                // PROFILBILD
                ProfileImageUrl = string.IsNullOrEmpty(user.ProfileImagePath)
                ? "/images/default-profile.png.jpg"
    :           user.ProfileImagePath,


                Skills = await _db.Skills.Where(s => s.UserId == myId).ToListAsync(),
                Utbildningar = await _db.Utbildningar.Where(u => u.UserId == myId).ToListAsync(),
                Erfarenheter = await _db.Erfarenheter.Where(e => e.UserId == myId).ToListAsync(),
                Projects = await _db.Projekts.Where(p => p.Collaborators.Any(c => c.Id == user.Id))
                .Select(p => p.Title)
                .ToListAsync()
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
                Address = user.Address ?? "",
                IsPublic = !user.IsPrivate,
                ProfileImageUrl = string.IsNullOrEmpty(user.ProfileImagePath)
                ? "/images/default-profile.png.jpg"
               : user.ProfileImagePath,


                Skills = await _db.Skills.Where(s => s.UserId == id).ToListAsync(),
                Utbildningar = await _db.Utbildningar.Where(u => u.UserId == id).ToListAsync(),
                Erfarenheter = await _db.Erfarenheter.Where(e => e.UserId == id).ToListAsync(),
                Projects = await _db.Projekts.Where(p => p.Collaborators.Any(c => c.Id == user.Id))
                .Select(p => p.Title)
                .ToListAsync()
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

            var user = await _db.Users.Include(u => u.CollaboratingProjects).FirstOrDefaultAsync(u => u.Id == myId);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var vm = new EditCvViewModel
            {
                FirstName = (user.FullName ?? "").Split(' ').FirstOrDefault() ?? "",
                LastName = (user.FullName ?? "").Split(' ').Skip(1).FirstOrDefault() ?? "",
                Address = user.Address ?? "",
                Email = user.Email ?? "",
                IsPublic = !user.IsPrivate,

                Skills = await _db.Skills.Where(s => s.UserId == myId).ToListAsync(),
                Utbildningar = await _db.Utbildningar.Where(u => u.UserId == myId).ToListAsync(),
                Erfarenheter = await _db.Erfarenheter.Where(e => e.UserId == myId).ToListAsync(),
            };

            //Hämta alla projekt för en lista med checkboxar, hämtar sedan ut de man tidigare kryssat i för att veta vad som ska vara ikryssat.
            var allProjects = await _db.Projekts.ToListAsync();
            var collabProjects = user.CollaboratingProjects.Select(p => p.Id).ToList();

            vm.Projects = new List<ProjectCheckBoxViewModel>();

            foreach (var project in allProjects)
            {
                vm.Projects.Add(new ProjectCheckBoxViewModel
                {
                    ProjectId = project.Id,
                    Title = project.Title,
                    IsSelected = collabProjects.Contains(project.Id)
                });
            }

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

            if (!ModelState.IsValid)
            {
                model.Skills = await _db.Skills.Where(s => s.UserId == myId).ToListAsync();
                model.Utbildningar = await _db.Utbildningar.Where(u => u.UserId == myId).ToListAsync();
                model.Erfarenheter = await _db.Erfarenheter.Where(e => e.UserId == myId).ToListAsync();

                return View(model);
            }

            var user = await _db.Users.Include(u => u.CollaboratingProjects).FirstOrDefaultAsync(u => u.Id == myId);
            if (user == null)
                return RedirectToAction("Login", "Account");

            // ===== Spara PROFILINFO (krav 4b) =====
            user.FullName = $"{model.FirstName} {model.LastName}".Trim();
            user.Address = model.Address;
            user.Email = model.Email;
            user.UserName = model.Email; 
            user.IsPrivate = !model.IsPublic;

            // PROFILBILD

            if (model.ProfileImage != null && model.ProfileImage.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(model.ProfileImage.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ProfileImage", "Endast JPG eller PNG är tillåtet");

                    model.Skills = await _db.Skills.Where(s => s.UserId == myId).ToListAsync();
                    model.Utbildningar = await _db.Utbildningar.Where(u => u.UserId == myId).ToListAsync();
                    model.Erfarenheter = await _db.Erfarenheter.Where(e => e.UserId == myId).ToListAsync();

                    return View(model);
                }

                var uploadsFolder = Path.Combine(_env.WebRootPath, "images", "profiles");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + extension;
                var filePath = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await model.ProfileImage.CopyToAsync(stream);

                user.ProfileImagePath = "/images/profiles/" + fileName;
            }

            //Gå med i valda projekt
            //Hämtar först ut id på valda projekt från vyn, sedan hämtas projekten från databasen via dessa id.
            var selectedProjects = model.Projects.Where(p => p.IsSelected).Select(p => p.ProjectId).ToList();
            var projectsToJoin = await _db.Projekts.Where(p => selectedProjects.Contains(p.Id)).ToListAsync();

            user.CollaboratingProjects.Clear();//Tar bort alla tidigare projekt användaren var med i för att undvika dupliceringar.

            foreach (var project in projectsToJoin)
            {
                user.CollaboratingProjects.Add(project);
            }


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


        // =========================
        // Delete skill
        // =========================

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DeleteSkillConfirm(int id)
        {
            var myId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(myId))
                return RedirectToAction("Login", "Account");

            var skill = await _db.Skills.FirstOrDefaultAsync(s => s.Id == id);
            if (skill == null)
                return NotFound();

            if (skill.UserId != myId)
                return Forbid();

            return View(skill);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSkillConfirmed(int id)
        {
            var myId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(myId))
                return RedirectToAction("Login", "Account");

            var skill = await _db.Skills.FirstOrDefaultAsync(s => s.Id == id);
            if (skill == null)
                return NotFound();

            if (skill.UserId != myId)
                return Forbid();

            _db.Skills.Remove(skill);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Edit));
        }


        // =========================
        // Redigera Skill
        // =========================

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditSkill(int id)
        {
            var myId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var skill = await _db.Skills.FirstOrDefaultAsync(s => s.Id == id && s.UserId == myId);
            if (skill == null) return NotFound();

            return View(skill);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSkill(Skill form)
        {
            ModelState.Remove("User");
            ModelState.Remove("UserId");

            var myId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var skill = await _db.Skills
                .FirstOrDefaultAsync(s => s.Id == form.Id && s.UserId == myId);

            if (skill == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View(form);

            skill.Name = form.Name;
            skill.Level = form.Level;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Edit));
        }



        // =========================
        // Redigera Utbildning
        // =========================


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditUtbildning(int id)
        {
            var myId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var utbildning = await _db.Utbildningar
                .FirstOrDefaultAsync(u => u.Id == id && u.UserId == myId);

            if (utbildning == null)
                return NotFound();

            return View(utbildning);
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUtbildning(Utbildning form)
        {
            ModelState.Remove("User");

            var myId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var utbildning = await _db.Utbildningar
                .FirstOrDefaultAsync(u => u.Id == form.Id && u.UserId == myId);

            if (utbildning == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View(form);

            utbildning.SchoolName = form.SchoolName;
            utbildning.Degree = form.Degree;
            utbildning.StartDate = form.StartDate;
            utbildning.EndDate = form.EndDate;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Edit));
        }


        // =========================
        // Redigera Erfarenhet
        // =========================

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditErfarenhet(int id)
        {
            var myId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var erfarenhet = await _db.Erfarenheter
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == myId);

            if (erfarenhet == null)
                return NotFound();

            return View(erfarenhet);
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditErfarenhet(Erfarenhet form)
        {
            ModelState.Remove("User");

            var myId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var erfarenhet = await _db.Erfarenheter
                .FirstOrDefaultAsync(e => e.Id == form.Id && e.UserId == myId);

            if (erfarenhet == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View(form);

            erfarenhet.CompanyName = form.CompanyName;
            erfarenhet.Position = form.Position;
            erfarenhet.Description = form.Description;
            erfarenhet.StartDate = form.StartDate;
            erfarenhet.EndDate = form.EndDate;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Edit));
        }






    }
}
