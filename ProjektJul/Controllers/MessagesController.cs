using Microsoft.AspNetCore.Authorization;
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

        // ===== KRAV: Skicka meddelande (från CV-sida) =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(MessageCreateVm vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["MessageError"] = "Du måste skriva ett meddelande.";
                return RedirectToAction("Details", "Cv", new { id = vm.ToUserId });
            }

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

        // ===== KRAV: Inkorg =====
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Inbox()
        {
            var myId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (myId == null) return RedirectToAction("Login", "Account");

            var messages = await _db.Messages
                .Where(m => m.ToUserId == myId)
                .OrderByDescending(m => m.SentAt)
                .Select(m => new InboxMessageVm
                {
                    Id = m.Id,
                    FromDisplayName = m.FromName ?? "Okänd",
                    Body = m.Body,
                    SentAt = m.SentAt,
                    IsRead = m.IsRead
                })
                .ToListAsync();

            return View(messages);
        }

        // ===== KRAV: Markera som läst =====
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkRead(int id)
        {
            var myId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (myId == null) return RedirectToAction("Login", "Account");

            var msg = await _db.Messages.FirstOrDefaultAsync(m => m.Id == id);
            if (msg == null) return NotFound();

            // Uni-nivå: säkerställ att användaren bara kan markera sina egna meddelanden
            if (msg.ToUserId != myId) return Forbid();

            if (!msg.IsRead)
            {
                msg.IsRead = true;
                await _db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Inbox));
        }
    }
}
