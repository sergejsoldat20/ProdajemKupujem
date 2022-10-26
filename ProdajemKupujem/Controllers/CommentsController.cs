using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProdajemKupujem.Data;
using ProdajemKupujem.Models;
using ProdajemKupujem.Models.Enums;

namespace ProdajemKupujem.Controllers
{
   
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Comments
        public async Task<IActionResult> Index(Guid productId)
        {
            var comments = from comment in _context.Comment.Include(c => c.Product).Include(c => c.User)
                           where comment.ProductId.Equals(productId)
                           orderby comment.CreatedDate
                           descending
                           select comment;
            
            return View(await comments.ToListAsync());
        }

        public async Task<IActionResult> Delete(Guid commentId) 
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = Consts.User)]
        public async Task<IActionResult> Create(Comment comment, Guid productId)
        {
            ClaimsPrincipal currentUser = this.User;
            var _comment = new Comment()
            {
                ProductId = productId,
                Text = comment.Text,
                UserId = Int32.Parse(_userManager.GetUserId(currentUser))
            };
            _context.Add(_comment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int commentId)
        {
            return View();
        }
       
    }
}
