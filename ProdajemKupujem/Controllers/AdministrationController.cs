using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Owin.Security;
using ProdajemKupujem.Data;
using Microsoft.EntityFrameworkCore;
using ProdajemKupujem.Models.Enums;
using ProdajemKupujem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using System.Runtime.ExceptionServices;

namespace ProdajemKupujem.Controllers
{
    [Authorize(Roles = Consts.Admin)]
    public class AdministrationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdministrationController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET/Administration
        public async Task<IActionResult> Index(string searchString) 
        {
            var nonAdminUsers = from user in _context.Users
                                join userRole in _context.UserRoles on user.Id equals userRole.UserId
                                join role in _context.Roles on userRole.RoleId equals role.Id
                                where role.Name.Equals(Consts.User)
                                select user;
            if (!String.IsNullOrEmpty(searchString))
            {
                nonAdminUsers = nonAdminUsers.Where(user => user.FirstName!.Contains(searchString) ||
                user.LastName!.Contains(searchString) ||
                (user.FirstName + " " + user.LastName)!.Contains(searchString) ||
                user.Email!.Contains(searchString));
            }
            return View(await nonAdminUsers.ToListAsync());
        }

        
        // POST Administraion/UpdateAdmin
        [HttpPost]
        public async Task<IActionResult> UpdateUserToAdmin(int Id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == Id);
            await _userManager.RemoveFromRoleAsync(user, Consts.User);
            await _userManager.AddToRoleAsync(user, Consts.Admin);
            return RedirectToAction(nameof(Index));
        }
        // POST Administration/Delete/1
        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == Id);
            if (user == null)
            {
                return Problem("User doesn't exist");
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
