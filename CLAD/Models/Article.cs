using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CLAD.Models
{
    public class Article
    {
        public int Id { get; set; }
        IdentityUser Author { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Comments { get; set; }
        public bool IsVisible { get; set; }
        public DateTime PublicationDate { get; set; }
        public List<ArticleTag> Tags { get; set; }

    }
}
