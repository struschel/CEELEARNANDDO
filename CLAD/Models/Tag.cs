using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CLAD.Models
{
    public class Tag
    {
        public virtual IList<ArticleTag> Articles { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
