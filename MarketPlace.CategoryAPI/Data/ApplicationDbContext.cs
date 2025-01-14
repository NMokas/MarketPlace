using MarketPlace.CategoryAPI.Models;
using MarketPlace.CategoryAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.CategoryAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ignore CategoryDto to prevent table creation
            modelBuilder.Ignore<CategoryDto>();
        }

    }
}
