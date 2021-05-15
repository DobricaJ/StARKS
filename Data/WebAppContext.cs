using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StARKS.Models;

namespace StARKS.Data
{
    public class WebAppContext : DbContext
    {
        public WebAppContext (DbContextOptions<WebAppContext> options)
            : base(options)
        {
            Database.EnsureCreated();
            
        
        }

        public DbSet<StARKS.Models.Student> Student { get; set; }

        public DbSet<StARKS.Models.Course> Course { get; set; }

        public DbSet<StARKS.Models.Mark> Mark { get; set; }
    }
}
