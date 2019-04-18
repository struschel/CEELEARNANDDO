using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CLAD.Models;
using CLAD.Data;

namespace CLAD.Controllers
{
    public class ConsultantController : Controller
    {
        UserManager<User> _userManager;
        RoleManager<IdentityRole> _roleManager;
        ApplicationDbContext _context;

        public ConsultantController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var consultants = await _userManager.GetUsersInRoleAsync("Consultant");

            List<ConsultantViewModel> list = new List<ConsultantViewModel>();

            foreach(User c in consultants)
            {
                ConsultantViewModel cvm = new ConsultantViewModel();
                cvm.User = c;
                cvm.Articles = _context.Articles.Where(a => a.AuthorId == c.Id).ToList();

                list.Add(cvm);

            }


            return View(list);
        }
    }
}