using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CLAD.Data;
using CLAD.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CLAD.Areas.Admin.Models;

namespace CLAD.Areas.Admin
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class QuestionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public QuestionsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Admin/Questions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Questions.ToListAsync());
        }

        // GET: Admin/Questions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // GET: Admin/Questions/Create
        public IActionResult Create()
        {
            return View(new AdminQuestionViewModel());
        }

        // POST: Admin/Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminQuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var question = new Question();

                question.Title = model.Title;
                question.Content = model.Content;
                question.IsVisible = model.IsVisible;

                question.Author = await _userManager.GetUserAsync(User);
                question.PublicationDate = DateTime.Now;
                question.Tags = new List<QuestionTag>();


                // Add tags
                if (!string.IsNullOrEmpty(model.Tags))
                {
                    var tags = model.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries);

                    if (tags.Any())
                    {
                        foreach (var tag in tags)
                        {
                            var dbTag = _context.Tags.FirstOrDefault(t => t.Name.ToLower() == tag.ToLower());
                            if (dbTag == null)
                            {
                                // Tag does not exist, create it
                                dbTag = new Tag();
                                dbTag.Name = tag;
                                _context.Tags.Add(dbTag);
                            }

                            var questionTag = new QuestionTag();
                            questionTag.Question = question;
                            questionTag.Tag = dbTag;

                            question.Tags.Add(questionTag);
                        }
                    }
                }

                _context.Add(question);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Admin/Questions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            var model = new AdminQuestionViewModel();

            model.Id = question.Id;
            model.Title = question.Title;
            model.Content = question.Content;
            model.IsVisible = question.IsVisible;
            model.Tags = question.GetTagsAsString();

            return View(model);
        }

        // POST: Admin/Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AdminQuestionViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var DBQuestion = await _context.Questions.FindAsync(id);

                    DBQuestion.Title = model.Title;
                    DBQuestion.Content = model.Content;
                    DBQuestion.IsVisible = model.IsVisible;


                    // Add tags
                    DBQuestion.Tags.Clear();

                    if (!string.IsNullOrEmpty(model.Tags))
                    {
                        var tags = model.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries);

                        if (tags.Any())
                        {
                            foreach (var tag in tags)
                            {
                                var dbTag = _context.Tags.FirstOrDefault(t => t.Name.ToLower() == tag.ToLower());
                                if (dbTag == null)
                                {
                                    // Tag does not exist, create it
                                    dbTag = new Tag();
                                    dbTag.Name = tag;
                                    _context.Tags.Add(dbTag);
                                }

                                var questionTag = new QuestionTag();
                                questionTag.Question = DBQuestion;
                                questionTag.Tag = dbTag;

                                DBQuestion.Tags.Add(questionTag);
                            }
                        }
                    }

                    _context.Update(DBQuestion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(model.Id))
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
            return View(model);
        }

        // GET: Admin/Questions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // POST: Admin/Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionExists(int id)
        {
            return _context.Questions.Any(e => e.Id == id);
        }
    }
}
