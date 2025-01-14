using MarketPlace.CouponAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.CouponAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Coupon> Coupons { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ignore CategoryDto to prevent table creation
            modelBuilder.Ignore<CouponDto>();
        }

    }
}
