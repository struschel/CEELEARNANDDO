using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CLAD.Models
{
    public class User : IdentityUser
    {
        public string DisplayName { get; set; }

        public string ProfileImageUrl { get; set; }

        public string GetProfileImageUrl()
        {
            if(string.IsNullOrEmpty(ProfileImageUrl))
            {
                return "http://placehold.it/50x50";
            }
            return ProfileImageUrl;
        }

        public string GetDisplayName()
        {
            if (string.IsNullOrEmpty(DisplayName))
            {
                return UserName;
            }
            return DisplayName;
        }

    }
}
