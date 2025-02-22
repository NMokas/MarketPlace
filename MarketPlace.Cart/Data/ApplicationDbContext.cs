﻿using MarketPlace.Cart.Models;
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


        }
    }
}
