using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CLAD.Data;
using CLAD.Models;
using CLAD.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CLAD.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public ArticlesController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;

            
        }

        // GET: Articles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Articles.Where(article => article.IsVisible).OrderByDescending(article => article.PublicationDate).ToListAsync());
        }

        // GET: Articles
        public async Task<IActionResult> Tag(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null) return NotFound();

            ViewBag.Tag = tag;

            return View(await _context.Articles
                                      .Where(article => article.IsVisible)
                                      .Where(article => article.Tags
                                                               .Any(articleTag => articleTag.TagId == id))
                                      .OrderByDescending(article => article.PublicationDate)
                                      .ToListAsync());
        }
        
        // GET: Articles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .Include(a => a.Author)
                .Include(a => a.Tags)
                .Include(a => a.Comments)
                .ThenInclude(c => c.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // GET: Articles/Details/5
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Details(int id, PostCommentModel model)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .FirstOrDefaultAsync(m => m.Id == id);


            if (article == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var comment = new ArticleComment();

                comment.Content = model.Message;
                comment.Author = await _userManager.GetUserAsync(User);
                comment.PublicationDate = DateTime.Now;

                // Instead of:
                //   article.Comments.Add(comment);
                // do:
                //   comment.Article = article;
                comment.Article = article;

                _context.Add(comment);

                await _context.SaveChangesAsync();

                ModelState.Clear();
            }

            return View(article);
        }

        // GET: Articles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Consultant")]
        public async Task<IActionResult> Create(ArticleCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);

                var article = new Article();

                article.Title = model.Title;
                article.Content = model.Content;
                article.Tags = new List<ArticleTag>();

                // Add tags
                if (!string.IsNullOrEmpty(model.Tags))
                {
                    var tags = model.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries);

                    if(tags.Any())
                    {
                        foreach(var tag in tags)
                        {
                            var dbTag = _context.Tags.FirstOrDefault(t => t.Name.ToLower() == tag.ToLower());
                            if(dbTag == null)
                            {
                                // Tag does not exist, create it
                                dbTag = new Tag();
                                dbTag.Name = tag;
                                _context.Tags.Add(dbTag);
                            }

                            var articleTag = new ArticleTag();
                            articleTag.Article = article;
                            articleTag.Tag = dbTag;

                            article.Tags.Add(articleTag);
                        }
                    }
                }

                article.AuthorId = userId;
                article.PublicationDate = DateTime.Now;


                _context.Add(article);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteComment(int id, int commentId)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null) return NotFound();

            var comment = article.Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment == null) return NotFound();

            article.Comments.Remove(comment);
            _context.Remove(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = id });
        }
    }
}
