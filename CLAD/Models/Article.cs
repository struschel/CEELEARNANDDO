using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CLAD.Models
{
    public class Article
    {
        public int Id { get; set; }

        public string Author { get; set; }

        public string ThumbnailPath { get; set; }

        [Required]
        [Display(Name = "Titel")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Inhoud")]
        public string Content { get; set; }

        [Display(Name = "Reacties")]
        public List<ArticleComment> Comments { get; set; }

        [Display(Name = "Zichtbaar")]
        public bool IsVisible { get; set; }

        [Display(Name = "Datum")]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}")]
        public DateTime PublicationDate { get; set; }

        public List<ArticleTag> Tags { get; set; }
    }
}
