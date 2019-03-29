using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CLAD.Models
{
    public class User
    {
        public int Id { get; set; }
        IdentityUser Gebruiker { get; set; }
        public string DisplayName { get; set; }
    }
}
