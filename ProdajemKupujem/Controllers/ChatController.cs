using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;
using ProdajemKupujem.Data;
using ProdajemKupujem.Models;
using Microsoft.EntityFrameworkCore;
using ProdajemKupujem.Migrations;
using Microsoft.AspNetCore.Authorization;

namespace ProdajemKupujem.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ChatController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET
        public async Task<IActionResult> Index()
        {
            int currentId = Int32.Parse(_userManager.GetUserId(this.User));
            var users = from user in _context.Users
                        where user.Id != currentId
                        select user;
            return View(await users.ToListAsync());
        }

        // GET
        public async Task<IActionResult> UserChat(int id)
        { 
            int currentId = Int32.Parse(_userManager.GetUserId(this.User)); 
            /*var receiver = from user in _context.Users
                           where user.Id == id
                           select user;
            */
            var receiver = _context.Users.Where(x => x.Id == id).FirstOrDefault();

            //Assert(receiver == null)
            if (receiver == null)
                return NotFound(); //raise exception

            ViewBag.CurrentUserEmail = _userManager.GetUserName(this.User);
            ViewBag.ReceiverId = id;
            ViewBag.ReceiverEmail = receiver.Email;
            ViewBag.CurrentUserFirstName = _context.Users.FirstOrDefault(u => u.Id == currentId).FirstName;

            var messages = await _context
                .Messages
                .Where(x => x.ChatId == Message.GetChatId(currentId, id))
                //.OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            return View(messages);
        }
     }
}
