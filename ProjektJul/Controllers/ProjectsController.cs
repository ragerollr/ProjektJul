using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt.Data.Identity;
using Projekt.Data.Persistence;
using Projekt.Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                .Include(p => p.Collaborators)
                .OrderByDescending(p => p.Id)
                .ToListAsync();

            var vm = projects.Select(p => new ProjectListItemViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                OwnerIsPrivate = p.User?.IsPrivate ?? false,
                OwnerId = p.UserId,
                PublishedAt = p.PublishedAt,
                OwnerName = (!isAuthenticated && (p.User?.IsPrivate == true)) ? "Privat" : p.User?.FullName ?? "Okänd",
                CollaboratorNames = p.Collaborators.Select(c => (!isAuthenticated && (c.IsPrivate)) ? "Privat" : c.FullName).ToList()
            }).ToList();

            return View(vm);
        }

        
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            //Hämta alla användare till listan för medarbetare
            var model = new ProjectFormViewModel();
            model.AvailableUsers = await _db.Users
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.FullName
                })
                .ToListAsync();
            return View(model);
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
                UserId = userId,
                PublishedAt = DateTime.UtcNow
            };

            //Koppla de medarbetarna man valt från listn till projekt.
            if (model.SelectedCollaboratorIds.Any())
            {
                var collaborators = await _db.Users
                    .Where(u => model.SelectedCollaboratorIds.Contains(u.Id))
                    .ToListAsync();
                
                project.Collaborators = collaborators;
            }

            _db.Projekts.Add(project);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var project = await _db.Projekts.Include(p => p.Collaborators).FirstOrDefaultAsync(p => p.Id == id);
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
                Description = project.Description,
                SelectedCollaboratorIds = project.Collaborators.Select(c => c.Id).ToList(),
            };
            vm.AvailableUsers = await _db.Users
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.FullName
                })
                .ToListAsync();

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

            var project = await _db.Projekts.Include(p => p.Collaborators).FirstOrDefaultAsync(p => p.Id == model.Id);
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

            //Uppdatera medarbetare
            var selectedCollaborators = await _db.Users
                .Where(u => model.SelectedCollaboratorIds.Contains(u.Id))
                .ToListAsync();
            project.Collaborators = selectedCollaborators;


            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
