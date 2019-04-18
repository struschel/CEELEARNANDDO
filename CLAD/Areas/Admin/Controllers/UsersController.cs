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
        UserManager<User> _userManager;
        RoleManager<IdentityRole> _roleManager;

        


        public UsersController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
          
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

            User user = await _userManager.FindByIdAsync(Id);

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

        // GET: Articles/Edit/5
        public async Task<IActionResult> Edit(string Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new UserEditModel();
            model.Id = user.Id;
            model.UserName = user.UserName;
            model.DisplayName = user.DisplayName;
            model.Email = user.Email;
            model.Description = user.Description;

            var roles = await _userManager.GetRolesAsync(user);
            var userRole = roles.FirstOrDefault();
            model.RoleName = userRole;

            List<SelectListItem> list = new List<SelectListItem>();

            foreach (var role in _roleManager.Roles)
            {
                list.Add(new SelectListItem() { Value = role.Name, Text = role.Name, Selected = (userRole == role.Name) });
                ViewBag.Roles = list;
            }
            return View(model);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string Id, UserEditModel user)
        {
            if (Id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var DBUser = await _userManager.FindByIdAsync(Id);
                    DBUser.DisplayName = user.DisplayName;
                    DBUser.UserName = user.UserName;
                    DBUser.Email = user.Email;
                    DBUser.Description = user.Description;

                    var roles = await _userManager.GetRolesAsync(DBUser);
                    var userRole = roles.FirstOrDefault();

                    if(userRole != user.RoleName)
                    {
                        await _userManager.RemoveFromRolesAsync(DBUser, roles);
                        await _userManager.AddToRoleAsync(DBUser, user.RoleName);
                    }

                    await _userManager.UpdateAsync(DBUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
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

            User user = await _userManager.FindByIdAsync(Id);
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

        private bool UserExists(string id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
