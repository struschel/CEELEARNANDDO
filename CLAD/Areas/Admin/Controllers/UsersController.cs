using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CLAD.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace CLAD.Models
{

    [Area("Admin")]

    [Authorize(Roles = "Admin")]

    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        UserManager<IdentityUser> _userManager;
        RoleManager<IdentityRole> _roleManager;
        


        public UsersController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            
            _userManager = userManager;
            _roleManager = roleManager;

          
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _userManager.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            IdentityUser user = await _userManager.FindByIdAsync(Id);

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                ViewData["Role"] = role;
            }


            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }



        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            IdentityUser user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
