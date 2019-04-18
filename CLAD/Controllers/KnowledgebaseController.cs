using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CLAD.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CLAD.Controllers
{
    public class KnowledgebaseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KnowledgebaseController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var questions = await _context.Questions.Where(q => q.IsVisible).ToListAsync();

            return View(questions);
        }

        // GET: Knowledgebase/{id}
        public async Task<IActionResult> Tag(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null) return NotFound();

            ViewBag.Tag = tag;

            return View(await _context.Questions
                                      .Where(question => question.IsVisible)
                                      .Where(question => question.Tags
                                                               .Any(questionTag => questionTag.TagId == id))
                                      .ToListAsync());
        }


        public async Task<IActionResult> Details(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if(question == null)
            {
                return NotFound();
            }

            return View(question);

        }
    }
}