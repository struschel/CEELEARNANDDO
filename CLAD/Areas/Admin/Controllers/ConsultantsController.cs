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
using Microsoft.AspNetCore.Mvc.Razor;

namespace CLAD.Models
{
    [Area("Admin")]
    public class ConsultantsController : Controller
    {
        UserManager<User> _userManager;
        RoleManager<IdentityRole> _roleManager;

        public ConsultantsController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
           
            return View(await _userManager.GetUsersInRoleAsync("Consultant"));
        }
    }
}