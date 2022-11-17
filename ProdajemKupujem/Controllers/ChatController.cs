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
            var receiver = from user in _context.Users
                           where user.Id == id
                           select user;
            ViewBag.CurrentUserEmail = _userManager.GetUserName(this.User);
            ViewBag.ReceiverId = id;
            ViewBag.ReceiverEmail = receiver.FirstOrDefault().Email;
            var messages = from message in _context.Messages.Include(u => u.Reciever).Include(u => u.Sender)
                           where message.SenderId == currentId &&
                           message.RecieverId == id
                           select message;
            return View(await messages.ToListAsync());
        }
    }
}
