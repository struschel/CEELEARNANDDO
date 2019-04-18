using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CLAD.Models;

namespace CLAD.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<CLAD.Models.Question> Questions { get; set; }
        public DbSet<CLAD.Models.Article> Articles { get; set; }
        public DbSet<CLAD.Models.User> User { get; set; }
        public DbSet<CLAD.Models.Mail> Mail { get; set; }
        public DbSet<CLAD.Models.Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<QuestionTag>(b =>
            {
                b.HasKey(questionTag => new { questionTag.QuestionId, questionTag.TagId });
            });
        }
    }
}
