using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CLAD.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }
        public string PhoneNumber { get; set; }
    }
}
