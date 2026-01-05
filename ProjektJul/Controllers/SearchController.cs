using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt.Data.Persistence;
using Projekt.Web.ViewModels;

namespace Projekt.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SearchController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // visar bara sökrutan
            return View(new SearchVm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(SearchVm vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var query = vm.Query.Trim();

            var isLoggedIn = User.Identity?.IsAuthenticated ?? false;

            // Grundquery: matcha på FullName (och fallback: Email om FullName saknas)
            var usersQuery = _db.Users.AsQueryable();

            // KRAV: anonym sökning får bara se offentliga profiler
            if (!isLoggedIn)
            {
                usersQuery = usersQuery.Where(u => !u.IsPrivate);
            }

            // Söklogik (case-insensitive via ToLower; funkar bra för kursnivå)
            query = query.ToLower();

            var results = await usersQuery
                .Where(u =>
                    (u.FullName ?? "").ToLower().Contains(query) ||
                    (u.Email ?? "").ToLower().Contains(query))
                .OrderBy(u => u.FullName ?? u.Email)
                .Select(u => new SearchResultItemVm
                {
                    UserId = u.Id,
                    FullName = u.FullName ?? u.Email ?? "Okänd",
                    IsPublic = !u.IsPrivate
                })
                .Take(50)
                .ToListAsync();

            ViewBag.Results = results;
            return View(vm);
        }
    }
}
