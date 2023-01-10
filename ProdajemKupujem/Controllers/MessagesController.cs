using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using EllipticCurve;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProdajemKupujem.Data;
using ProdajemKupujem.Models;
using ProdajemKupujem.Models.DTO;

namespace ProdajemKupujem.Controllers
{
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MessagesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var receivers = _context.Users.ToList().Where(x => x.Id != user.Id).Select(x => new ReceiverDTO(x));
            return View(receivers);
        }

        [Authorize]
        [HttpGet("[controller]/GetForChat/{userId}")]
        public async Task<string> GetForChat(string userId)
        {
            var user = await _userManager.GetUserAsync(User);
            var messages = _context
                 .Message
                 .Where(x => x.ChatId.Equals(Message.GenerateChatId(userId, user.Id.ToString())))
                 .OrderBy(x => x.CreationTime)
                 .ToList();
            return JsonSerializer.Serialize(messages);
        }


        [Authorize]
        [HttpPost("[controller]/SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageDTO payload)
        {
            var user = await _userManager.GetUserAsync(User);

            if (_context.Users.Find(Int32.Parse(payload.ReceiverId)) == null)
            {
                return NotFound();
            }

            Message message = new();
            message.SetSenderId(user.Id.ToString());
            message.SetReceiverId(payload.ReceiverId);
            message.Text = payload.Text;

            _context.Add(message);
            _context.SaveChanges();

            return Ok();
        }
    }


}
