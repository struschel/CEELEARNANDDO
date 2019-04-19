using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CLAD.Areas.Admin.Models
{
    public class AdminQuestionViewModel
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Tags { get; set; }

        public bool IsVisible { get; set; }
    }
}
