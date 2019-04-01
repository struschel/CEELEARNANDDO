
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CLAD.Models
{
    public class CLADContext : DbContext
    {
        public CLADContext(DbContextOptions<CLADContext> options)
            : base(options)
        {
        }

        public DbSet<CLAD.Models.Mail> Mail { get; set; }
    }
}