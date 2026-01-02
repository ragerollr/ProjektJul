using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt.Data.Models;
using Projekt.Data.Persistence;

namespace Projekt.Web.Controllers
{
    public class MessageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MessageController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Visa inbox
        public IActionResult Inbox()
        {
            var messages = _context.Messages
                .Include(m => m.Sender) // Ladda med avsändarens namn
                .OrderByDescending(m => m.SentAt)
                .ToList();

            return View(messages);
        }

        // Visa formulär för att skicka meddelande
        [HttpGet]
        public IActionResult SendMessage(string receiverId)
        {
            var message = new Message
            {
                ReceiverId = receiverId
            };
            return View(message);
        }

        // Skicka meddelande
        [HttpPost]
        public async Task<IActionResult> SendMessage(Message message)
        {
            if (ModelState.IsValid)
            {
                // Om användaren är inloggad, fyll i deras ID automatiskt
                if (User.Identity.IsAuthenticated)
                {
                    var user = await _userManager.GetUserAsync(User);
                    message.SenderId = user.Id;
                }
                else
                {
                    // Om anonym, kontrollera att avsändarnamn finns (SenderId används för namn här)
                    if (string.IsNullOrWhiteSpace(message.SenderId))
                    {
                        ModelState.AddModelError("SenderId", "Du måste skriva ditt namn som avsändare.");
                        return View(message);
                    }
                }

                message.SentAt = DateTime.Now;

                _context.Messages.Add(message);
                _context.SaveChanges();

                return RedirectToAction("Inbox");
            }

            return View(message);
        }
    }
}