using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CLAD.Models
{
    public class User
    {
        public int Id { get; set; }
        public IdentityUser Gebruiker { get; set; }
        public string DisplayName { get; set; }
    }
}
