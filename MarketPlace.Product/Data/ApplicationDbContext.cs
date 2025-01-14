using Microsoft.EntityFrameworkCore;
using MarketPlace.ProductAPI.Models;
using MarketPlace.ProductAPI.Models.Dto;

namespace MarketPlace.ProductAPI.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ignore CategoryDto to prevent table creation
            modelBuilder.Ignore<CategoryDto>();
        }
    }
}
