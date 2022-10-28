using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> Index(Guid id)
        {
            var comments = from comment in _context.Comment.Include(c => c.Product).Include(c => c.User)
                           where comment.ProductId.Equals(id)
                           select comment;
            return View(await comments.ToListAsync());
        }

        // GET: Comments/Create
        [Authorize(Roles = Consts.User)]
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = Consts.User)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id, Comment comment)
        {
            ClaimsPrincipal currentUser = this.User;
            var _comment = new Comment()
            {
                ProductId = id,
                UserId = Int32.Parse(_userManager.GetUserId(currentUser)),
                Text = comment.Text
            };
            _context.Add(_comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index",new { id = id});
        }

        // GET: Comments/Edit/5
        [Authorize(Roles = Consts.User)]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Comment == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id", comment.ProductId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", comment.UserId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Consts.User)]
        public async Task<IActionResult> Edit(Guid id,[Bind("Id,UserId,Text,ProductId,RowVersion")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }
            try
            {
                _context.Update(comment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(comment.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Index", new {id = comment.ProductId });
        }
        [Authorize(Roles = Consts.User)]
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var comment = _context.Comment.FirstOrDefault(c => c.Id.Equals(id));
            if (UserHasProduct(id))
            {
                _context.Comment.Remove(comment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", new { id = comment.ProductId });
         }

        private bool CommentExists(Guid id)
        {
          return _context.Comment.Any(e => e.Id == id);
        }

        public bool UserHasProduct(Guid productId) 
        {
            var currentUserId = Int32.Parse(_userManager.GetUserId(this.User));
            var currentUserProducts = from product in _context.Product
                                      where product.UserId == currentUserId
                                      select product;
            return currentUserProducts.Any(p => p.Id == productId);
        }
    }
}
