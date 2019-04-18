using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CLAD.Models
{
    public class User : IdentityUser
    {
        public string DisplayName { get; set; }


        public string ProfileImageUrl { get; set; }

        [Display(Name = "Omschrijving")]
        public string Description { get; set; }

        public virtual IList<Article> Articles { get; set; }

        public virtual IList<Question> Questions { get; set; }

        
        public string GetDescription()
        {
            if (string.IsNullOrWhiteSpace(Description))
            {
                return "Default Description";
            }
            return Description;
        }

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
