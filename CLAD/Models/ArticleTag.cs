﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CLAD.Models
{
    public class ArticleTag
    {
        public int Id { get; set; }
        
        public virtual Article Article { get; set; }

        public int TagId { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
