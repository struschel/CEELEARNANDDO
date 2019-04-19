using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CLAD.Models
{
    public class UserEditModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string DisplayName { get; set; }

        public string RoleName { get; set; }

        public string Email { get; set; }

        public string Description { get; set; }

    }
}
