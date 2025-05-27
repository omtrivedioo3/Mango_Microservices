﻿using Mango.Services.CouponAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Coupon> Coupons { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Coupon>().HasData(
                new Coupon
                {
                    CouponId = 1,
                    CouponCode = "10OFF",
                    MinAmount = 100,
                    DiscountAmount = 10
                },
                new Coupon
                {
                    CouponId = 2,
                    CouponCode = "20OFF",
                    MinAmount = 200,
                    DiscountAmount = 20
                }
                );
        }
    }
   
}
