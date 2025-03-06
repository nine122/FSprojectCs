using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FullstackHW.Models
{
    public class ProductDbContext:DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options):base(options)
        {
            
        }
        public DbSet<ProductViewModel> NorthRegion { get; set; }
    }
}