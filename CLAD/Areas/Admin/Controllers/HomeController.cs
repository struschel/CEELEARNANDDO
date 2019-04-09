using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CLAD.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Area("Admin")]

        [Authorize(Roles = "Admin")]

        public IActionResult Index()
        {
            return View();
        }
    }
}