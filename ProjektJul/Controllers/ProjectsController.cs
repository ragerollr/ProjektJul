using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt.Data.Identity;
using Projekt.Data.Persistence;
using Projekt.Web.ViewModels;

namespace Projekt.Web.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProjectsController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var isAuthenticated = User.Identity?.IsAuthenticated == true;

            var projects = await _db.Projekts
                .Include(p => p.User)
                .OrderByDescending(p => p.Id)
                .ToListAsync();

            if (!isAuthenticated)
            {
                projects = projects
                    .Where(p => p.User is not null && !p.User.IsPrivate)
                    .ToList();
            }

            var vm = projects.Select(p => new ProjectListItemViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                OwnerName = p.User?.FullName ?? "Okänd",
                OwnerIsPrivate = p.User?.IsPrivate ?? false
            }).ToList();

            return View(vm);
        }

        
        [HttpGet]
        public IActionResult Create()
        {
            return View(new ProjectFormViewModel());
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                TempData["Alert"] = "Du måste vara inloggad.";
                return RedirectToAction("Index", "Projects");
                //return Forbid();
            }

            var project = new CVSite.Data.Models.Projects
            {
                Title = model.Title,
                Description = model.Description,
                UserId = userId
            };

            _db.Projekts.Add(project);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var project = await _db.Projekts.FirstOrDefaultAsync(p => p.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (project.UserId != userId)
            {
                return Forbid();
            }

            var vm = new ProjectFormViewModel
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description
            };

            return View(vm);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProjectFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var project = await _db.Projekts.FirstOrDefaultAsync(p => p.Id == model.Id);
            if (project == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (project.UserId != userId)
            {
                return Forbid();
            }

            project.Title = model.Title;
            project.Description = model.Description;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
