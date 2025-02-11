using Microsoft.EntityFrameworkCore;
using StockAPI.Models;

namespace StockAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<Stock> Stock { get; set; }
        public DbSet<Comment> Comment { get; set; }
    }
}
