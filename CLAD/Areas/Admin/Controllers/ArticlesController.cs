using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CLAD.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CLAD.Areas.Admin.Models;

namespace CLAD.Models
{
    
    [Area ("Admin")]   
    [Authorize(Roles = "Admin")]

    public class ArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> userManager;

        public ArticlesController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        // GET: Articles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Articles.ToListAsync());
        }

        // GET: Articles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // GET: Articles/Create
        public IActionResult Create()
        {
            return View(new AdminArticleViewModel());
        }

        // POST: Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminArticleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = userManager.GetUserId(User);

                var article = new Article();

                article.Title = model.Title;
                article.Content = model.Content;
                article.IsVisible = model.IsVisible;
                
                article.AuthorId = userId;
                article.PublicationDate = DateTime.Now;
                article.Tags = new List<ArticleTag>();


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

                            var articleTag = new ArticleTag();
                            articleTag.Article = article;
                            articleTag.Tag = dbTag;

                            article.Tags.Add(articleTag);
                        }
                    }
                }

                _context.Add(article);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Articles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            var model = new AdminArticleViewModel();

            model.Id = article.Id;
            model.Title = article.Title;
            model.Content = article.Content;
            model.IsVisible = article.IsVisible;
            model.Tags = string.Join(',', article.Tags.Select(t => t.Tag.Name));

            return View(model);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AdminArticleViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var DBArticle = _context.Articles.Find(id);

                    DBArticle.Title = model.Title;
                    DBArticle.Content = model.Content;
                    DBArticle.IsVisible = model.IsVisible;


                    // Add tags
                    DBArticle.Tags.Clear();

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

                                var articleTag = new ArticleTag();
                                articleTag.Article = DBArticle;
                                articleTag.Tag = dbTag;

                                DBArticle.Tags.Add(articleTag);
                            }
                        }
                    }

                    _context.Update(DBArticle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(model.Id))
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

        // GET: Articles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _context.Articles.FindAsync(id);

            _context.RemoveRange(article.Comments);
            _context.RemoveRange(article.Tags);

            _context.Articles.Remove(article);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }
    }
}
