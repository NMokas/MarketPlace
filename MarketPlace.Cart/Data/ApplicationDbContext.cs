using MarketPlace.Cart.Models;
using MarketPlace.Cart.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Cart.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<CartDetails> CartDetails{ get; set; }
        public DbSet<CartHeader> CartHeaders{ get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ignore CategoryDto to prevent table creation
            modelBuilder.Ignore<CategoryDto>();
            modelBuilder.Ignore<ResponseDto>();
            modelBuilder.Ignore<ProductDto>();
            modelBuilder.Ignore<CartDetailsDto>();
            modelBuilder.Ignore<CartHeaderDto>();
            modelBuilder.Ignore<CouponDto>();
            modelBuilder.Ignore<CartDto>();
        }

    }
}
