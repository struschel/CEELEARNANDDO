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
using CLAD.ViewModels;

namespace CLAD.Controllers
{
    [Authorize]
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
            List<Question> questions = new List<Question>();

            if (User.IsInRole("Consultant") || User.IsInRole("Admin"))
            {
                questions.AddRange(await _context.Questions.ToListAsync());
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);

                questions.AddRange(await _context.Questions.Where(q => q.Author.Id == user.Id).ToListAsync());
            }

            return View(questions);
        }

        // GET: Questions
        public async Task<IActionResult> Tag(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null) return NotFound();

            ViewBag.Tag = tag;

            var questions = new List<Question>();

            if (User.IsInRole("Consultant") || User.IsInRole("Admin"))
            {
                questions.AddRange(await _context.Questions
                                      .Where(question => question.Tags
                                                               .Any(questionTag => questionTag.TagId == id))
                                                               .ToListAsync());
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);

                questions.AddRange(await _context.Questions.Where(q => q.Author.Id == user.Id)
                                                           .Where(question => question.Tags
                                                                .Any(questionTag => questionTag.TagId == id))
                                                               .ToListAsync());
            }

            return View(questions);
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

            if (!await UserOwnsQuestionAsync(question))
            {
                return NotFound();
            }

            return View(question);
        }

        // GET: Admin/Questions/Create
        public IActionResult Create()
        {
            return View(new QuestionCreateViewModel());
        }

        // POST: Admin/Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuestionCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var question = new Question();

                question.Title = model.Title;
                question.Content = model.Content;
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

                question.Author = await _userManager.GetUserAsync(User);
                question.PublicationDate = DateTime.Now;

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

            if (!await UserOwnsQuestionAsync(question))
            {
                return NotFound();
            }

            var model = new QuestionCreateViewModel();

            model.Id = question.Id;
            model.Title = question.Title;
            model.Content = question.Content;
            model.Tags = string.Join(',', question.Tags.Select(t => t.Tag.Name));

            return View(model);
        }

        // POST: Admin/Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, QuestionCreateViewModel model)
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

                    if (!await UserOwnsQuestionAsync(DBQuestion))
                    {
                        return NotFound();
                    }

                    DBQuestion.Title = model.Title;
                    DBQuestion.Content = model.Content;


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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Consultant")]
        public async Task<IActionResult> AddAnswer(int id, [Bind("Content")] Answer answer)
        {
            if (ModelState.IsValid)
            {
                var DBQuestion = await _context.Questions.FindAsync(id);
                if (DBQuestion == null)
                {
                    return NotFound();
                }

                // Make the answer and save it to the context
                var DBAnswer = new Answer();
                DBAnswer.Question = DBQuestion;
                DBAnswer.Author = await _userManager.GetUserAsync(User);
                DBAnswer.Content = answer.Content;
                DBAnswer.PublicationDate = DateTime.Now;

                DBQuestion.Answers.Add(DBAnswer);
                _context.Update(DBQuestion);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", new { id = id });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAnswer(int id, int answerId)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null) return NotFound();

            var answer = question.Answers.FirstOrDefault(a => a.Id == answerId);
            if (answer == null) return NotFound();

            question.Answers.Remove(answer);
            _context.Remove(answer);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = id });
        }

        private bool QuestionExists(int id)
        {
            return _context.Questions.Any(e => e.Id == id);
        }

        private async Task<bool> UserOwnsQuestionAsync(Question question)
        {
            if (User.IsInRole("Consultant") || User.IsInRole("Admin")) return true;

            var user = await _userManager.GetUserAsync(User);

            if (question == null || user == null) return false;
            if (question.Author.Id == user.Id) return true;

            return false;
        }
    }
}
