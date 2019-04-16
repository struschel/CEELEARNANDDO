using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CLAD.Models
{
    public class UserProfileEditModel
    {
        public IFormFile AvatarImage { get; set; }
        public string DisplayName { get; set; }
    }
}
