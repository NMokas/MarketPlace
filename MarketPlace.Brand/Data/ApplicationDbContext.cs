using MarketPlace.AuthAPI.Models;
using MarketPlace.AuthAPI.Models.Dto;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.AuthAPI.Data
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ignore CategoryDto to prevent table creation
            modelBuilder.Ignore<LoginRequestDto>();
            modelBuilder.Ignore<LoginResponseDto>();
            modelBuilder.Ignore<RegistrationRequestDto>();
            modelBuilder.Ignore<ResponseDto>();
            modelBuilder.Ignore<UserDto>();
        }
    }
}
