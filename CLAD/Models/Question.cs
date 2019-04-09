using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CLAD.Models
{
    public class Question
    {
        public int Id { get; set; }
        public List<Answer> Answers { get; set; }
        public User Author { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsVisible { get; set; }
        public DateTime PublicationDate { get; set; }
        public List<QuestionTag> Tags { get; set; }
    }
}
