using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Product_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product_Management_System.Context
{
    public class AppDbContext : IdentityDbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Image> Images { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
    }

}
