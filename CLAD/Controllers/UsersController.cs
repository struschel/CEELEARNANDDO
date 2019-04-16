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
using CLAD.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace CLAD.Controllers
{

    [Authorize]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        UserManager<User> _userManager;
        RoleManager<IdentityRole> _roleManager;



        public UsersController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext applicationDbContext)
        {

            _userManager = userManager;
            _roleManager = roleManager;
            _context = applicationDbContext;

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

        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new UserProfileEditModel();
            model.DisplayName = user.DisplayName;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserProfileEditModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                if (model.AvatarImage != null && model.AvatarImage.Length > 0)
                {

                    // full path to file in temp location
                    var fileExtension = Path.GetExtension(model.AvatarImage.FileName);
                    var relativeUrl = "AvatarImages/" + User.Identity.Name + "." + fileExtension;
                    var filePath = Url.Content("wwwroot/" + relativeUrl);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.AvatarImage.CopyToAsync(stream);
                    }
                    user.ProfileImageUrl = "/" + relativeUrl;
                }

                user.DisplayName = model.DisplayName;

                _context.Update(user);
                await _context.SaveChangesAsync();
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.



            return View();
        }
    }
}
