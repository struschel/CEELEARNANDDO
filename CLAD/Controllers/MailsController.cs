using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CLAD.Models;

namespace ContactForm.Controllers
{
    public class MailsController : Controller
    {
        private readonly CLADContext _context;

        public MailsController(CLADContext context)
        {
            _context = context;
        }

        // GET: Mails
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewBag.NaamSortParm = sortOrder == "naam" ? "naam_desc" : "naam";
            ViewBag.EmailSortParm = sortOrder == "email" ? "email_desc" : "email";
            ViewBag.OnderwerpSortParm = sortOrder == "onderwerp" ? "onderwerp_desc" : "onderwerp";
            var mail = from m in _context.Mail
                       select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                mail = mail.Where(s => s.Naam.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "naam":
                    mail = mail.OrderBy(s => s.Naam);
                    break;
                case "naam_desc":
                    mail = mail.OrderByDescending(s => s.Naam);
                    break;
                case "email":
                    mail = mail.OrderBy(s => s.Email);
                    break;
                case "email_desc":
                    mail = mail.OrderByDescending(s => s.Email);
                    break;
                case "onderwerp":
                    mail = mail.OrderBy(s => s.Onderwerp);
                    break;
                case "onderwerp_desc":
                    mail = mail.OrderByDescending(s => s.Onderwerp);
                    break;
                default:
                    mail = mail.OrderBy(s => s.Id);
                    break;
            }
            return View(await mail.ToListAsync());
        }

        // GET: Mails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mail = await _context.Mail
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mail == null)
            {
                return NotFound();
            }

            return View(mail);
        }

        // GET: Mails/Create
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Sent()
        {
            return View();
        }

        // POST: Mails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Naam,Email,TelefoonNr,Onderwerp,Bericht")] Mail mail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mail);
                await _context.SaveChangesAsync();
                var body = "<b><p>Email From: {0} ({1})</p></b><b><p>Telefoonnummer:</p></b> {2} <b><p>Onderwerp:</p></b> {3}<b><p>Message:</p></b><p>{4}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("bowelter@hotmail.com"));  // replace with valid value 
                message.From = new MailAddress("bowelterbot@hotmail.com");  // replace with valid value
                message.Subject = "E-mail CeeLearnAndDo Contactform";
                message.Body = string.Format(body, mail.Naam, mail.Email, mail.TelefoonNr, mail.Onderwerp, mail.Bericht);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "bowelterbot@hotmail.com",  // replace with valid value
                        Password = "QAk4I5L40knxUYIe8fip"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp-mail.outlook.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                    return RedirectToAction("Sent");
                }
            }
            return View(mail);
        }

        // GET: Mails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mail = await _context.Mail.FindAsync(id);
            if (mail == null)
            {
                return NotFound();
            }
            return View(mail);
        }

        // GET: Mails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mail = await _context.Mail
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mail == null)
            {
                return NotFound();
            }

            return View(mail);
        }

        // POST: Mails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mail = await _context.Mail.FindAsync(id);
            _context.Mail.Remove(mail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MailExists(int id)
        {
            return _context.Mail.Any(e => e.Id == id);
        }
    }
}
