using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VeggiFoodAPI.Models.DTOs;
using System.Data;
using System.Reflection.Emit;

namespace VeggiFoodAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,int>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        //public DbSet<Basket> Baskets { get; set; }
        //public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("Users", "dbo");
            builder.Entity<ApplicationRole>().ToTable("Roles", "dbo");

            builder.Entity<ApplicationUser>()
                .HasOne(a => a.Address)
                .WithOne()
                .HasForeignKey<UserAddress>(a => a.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationRole>()
            .HasData(
                    new ApplicationRole { Id = 1, Name = "Member", NormalizedName = "MEMBER" },
                    new ApplicationRole { Id = 2, Name = "Admin", NormalizedName = "ADMIN" }
                );
        }
    }
}
