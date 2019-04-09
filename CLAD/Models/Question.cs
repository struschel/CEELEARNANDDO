using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CLAD.Models
{
    public class Question
    {
        public int Id { get; set; }

        public List<Answer> Answers { get; set; }

        public User Author { get; set; }

        [Display(Name = "Titel")]
        public string Title { get; set; }

        [Display(Name = "Inhoud")]
        public string Content { get; set; }

        [Display(Name = "Zichtbaar")]
        public bool IsVisible { get; set; }

        [Display(Name = "Datum")]
        public DateTime PublicationDate { get; set; }

        public List<QuestionTag> Tags { get; set; }
    }
}
