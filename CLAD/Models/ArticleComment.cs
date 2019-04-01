﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CLAD.Models
{
    public class ArticleComment
    {
        public int Id { get; set; }
        IdentityUser Author { get; set; }
        public int ArticleId { get; set; }
        public string Content { get; set; }
        public DateTime PublicationDate { get; set; }
    }
}