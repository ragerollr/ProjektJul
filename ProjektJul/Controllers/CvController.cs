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

        public CvController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Min egen profil (krav 4/6)
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> MyProfile()
        {
            var myId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (myId == null) return RedirectToAction("Login", "Account");

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == myId);
            if (user == null) return RedirectToAction("Login", "Account");

            var vm = new CvViewModel
            {
                UserId = user.Id,
                FullName = user.FullName ?? user.Email ?? "Okänd",
                Email = user.Email ?? "",
                Address = "",
                IsPublic = !user.IsPrivate,
                ProfileImageUrl = "/images/default-profile.png",
                Skills = new(),
                Educations = new(),
                Experiences = new(),
                Projects = new()
            };

            return View(vm); // Views/Cv/MyProfile.cshtml
        }

        // Visar CV för en specifik användare (krav 5/6/7/11)
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound();

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
                Skills = new(),
                Educations = new(),
                Experiences = new(),
                Projects = new()
            };

            return View("MyProfile", vm); // återanvänd vyn
        }
    }
}
