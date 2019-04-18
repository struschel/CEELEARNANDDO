using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CLAD.Areas.Admin.Models
{
    public class AdminArticleViewModel
    {
        // Id,Title,Content,Comments,IsVisible,PublicationDate,AuthorId

        public int Id { get; set; }

        [Required]
        [Display(Name = "Titel")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Inhoud")]
        public string Content { get; set; }

        [Display(Name = "Zichtbaar")]
        public bool IsVisible { get; set; }

        public string Tags { get; set; }
    }
}
