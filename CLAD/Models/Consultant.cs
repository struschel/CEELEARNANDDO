using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CLAD.Models
{
    public class Consultant
    {
        public User User { get; set; }
        [Display(Name = "Omschrijving")]
        public string Description { get; set; }

    }
}
