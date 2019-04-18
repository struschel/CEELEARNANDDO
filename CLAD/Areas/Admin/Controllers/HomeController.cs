using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CLAD.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CLAD.Models;
using System.Collections;

namespace CLAD.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext _context;
        UserManager<User> _userManager;
        RoleManager<IdentityRole> _roleManager;

        public HomeController(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;

        }

        [Area("Admin")]

        [Authorize(Roles = "Admin")]

       

        public IActionResult Index()
        {
            ViewData["ArticleCount"] = _context.Articles.Count();
           var cons = _userManager.GetUsersInRoleAsync("Consultant").Result;
            ViewData["ConsultantCount"] = cons.Count();
            return View();
        }
    }
}