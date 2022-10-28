using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using ProdajemKupujem.Data;
using ProdajemKupujem.Models;

namespace ProdajemKupujem.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class ApplicationUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationUsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ApplicationUsers
        public async Task<IActionResult> Index()
        {
            ClaimsPrincipal currentUser = this.User;
            int currentUserId = Int32.Parse(_userManager.GetUserId(currentUser));
            var products = from product in _context.Product
                           where product.UserId == currentUserId
                           select product;

            return View(await products.ToListAsync());
        }

        public ApplicationUser GetCurrentUser() { 
        
            ClaimsPrincipal currentUser = this.User;
            int userId = Int32.Parse(_userManager.GetUserId(currentUser));
            return _context.Users.FirstOrDefault(x => x.Id == userId);
        }
    } 
}
