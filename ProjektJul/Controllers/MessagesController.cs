using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt.Data.Identity;
using Projekt.Data.Models;
using Projekt.Data.Persistence;
using Projekt.Web.ViewModels;
using System.Security.Claims;

namespace Projekt.Web.Controllers
{
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public MessagesController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(MessageCreateVm vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["MessageError"] = "Du måste skriva ett meddelande.";
                return RedirectToAction("Details", "Cv", new { id = vm.ToUserId });
            }

            // recipient exists?
            var recipientExists = await _db.Users.AnyAsync(u => u.Id == vm.ToUserId);
            if (!recipientExists) return NotFound();

            var isLoggedIn = User.Identity?.IsAuthenticated ?? false;

            string? fromUserId = null;
            string? fromName;

            if (isLoggedIn)
            {
                fromUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var sender = await _userManager.FindByIdAsync(fromUserId!);
                fromName = !string.IsNullOrWhiteSpace(sender?.FullName)
                    ? sender!.FullName
                    : sender?.Email ?? "Inloggad användare";
            }
            else
            {
                if (string.IsNullOrWhiteSpace(vm.FromName))
                {
                    TempData["MessageError"] = "Du måste ange ditt namn om du är anonym.";
                    return RedirectToAction("Details", "Cv", new { id = vm.ToUserId });
                }

                fromName = vm.FromName.Trim();
            }

            var message = new Message
            {
                ToUserId = vm.ToUserId,
                FromUserId = fromUserId,
                FromName = fromName,
                Body = vm.Body.Trim(),
                SentAt = DateTime.UtcNow,
                IsRead = false
            };

            _db.Messages.Add(message);
            await _db.SaveChangesAsync();

            TempData["MessageSuccess"] = "Meddelandet skickades!";
            return RedirectToAction("Details", "Cv", new { id = vm.ToUserId });
        }
    }
}
