using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt.Data.Persistence;
using Projekt.Web.ViewModels;

namespace Projekt.Web.Controllers
{
    public class CvController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CvController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Visar CV för en specifik användare (krav #5/#7 och behövs för #11)
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound();

            var vm = new CvViewModel
            {
                UserId = user.Id,
                FullName = user.FullName ?? user.Email ?? "Okänd",
                Email = user.Email ?? "",
                Address = "", // ni kan koppla senare
                IsPublic = !user.IsPrivate,
                ProfileImageUrl = "/images/default-profile.png",
                Skills = new(),
                Educations = new(),
                Experiences = new(),
                Projects = new()
            };

            return View("MyProfile", vm); // återanvänd din view
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> MyProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return RedirectToAction("Login", "Account");

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return RedirectToAction("Logout", "Account");

            var vm = new CvViewModel
            {
                UserId = user.Id,
                FullName = user.FullName ?? user.Email ?? "Okänd",
                Email = user.Email ?? string.Empty,
                Address = string.Empty,
                IsPublic = !user.IsPrivate,
                ProfileImageUrl = "/images/default-profile.png",
                Skills = new(),
                Educations = new(),
                Experiences = new(),
                Projects = new()
            };

            return View(vm);
        }

        //public IActionResult MyProfile()
        //{
        //   // Din hårdkodade variant kan vara kvar tills ni kopplar riktig profil.
        //   var vm = new CvViewModel
        //   {
        //       UserId = "", // tom nu
        //       FullName = "Elin Sundell",
        //       Email = "elin@test.se",
        //       Address = "Östersund",
        //       IsPublic = true,
        //       ProfileImageUrl = "/images/default-profile.png",
        //       Skills = new() { "C#", "ASP.NET", "SQL" },
        //       Educations = new() { "Uppsala universitet" },
        //       Experiences = new() { "Praktik – Backend" },
        //       Projects = new() { "CV-portal", "API-projekt" }
        //   };

        //   return View(vm);
        //}
    }
}
