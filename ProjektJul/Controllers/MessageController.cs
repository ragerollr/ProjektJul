using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt.Data.Identity;
using Projekt.Data.Models;
using Projekt.Data.Persistence;

namespace Projekt.Web.Controllers
{
    public class MessageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MessageController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Visa formulär för att skicka meddelande
        [HttpGet]
        public IActionResult SendMessage(string receiverId)
        {
            var vm = new Message
            {
                ReceiverId = receiverId
            };
            return View(vm);
        }

        // Skicka meddelande
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage(Message message)
        {
            if (!ModelState.IsValid)
                return View(message);

            // Om användaren är inloggad, fyll i deras ID automatiskt
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                message.SenderId = user.Id;
            }
            else
            {
                // Om anonym, måste skriva namn
                if (string.IsNullOrWhiteSpace(message.SenderName))
                {
                    ModelState.AddModelError("SenderName", "Du måste ange ditt namn.");
                    return View(message);
                }
            }

            message.SentAt = DateTime.Now;
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Meddelandet skickades!";
            return RedirectToAction("SendMessage", new { receiverId = message.ReceiverId }); //test
        }
    }
}
