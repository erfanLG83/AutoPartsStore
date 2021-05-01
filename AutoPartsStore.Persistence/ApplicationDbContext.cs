using AutoPartsStore.Domain.Auth;
using AutoPartsStore.Domain.Entities;
using AutoPartsStore.Persistence.Mapping;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace AutoPartsStore.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<AppUser,AppRole,string>
    {
        public ApplicationDbContext()
        {

        }
        public ApplicationDbContext(DbContextOptions options)
            :base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCard> ProductCards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Server=.;Database=AutoPartsStoreDB;trusted_Connection=True");
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration<Product>(new ProductMapping());
            builder.ApplyConfiguration<Order>(new OrderMapping());
            builder.ApplyConfiguration<AppUser>(new UserMapping());
            builder.Entity<AppUser>()
                .ToTable("AppUsers");
            builder.Entity<AppRole>()
                .ToTable("AppRoles");
            builder.Entity<Category>()
                .HasKey(n => n.Id);
            builder.Entity<Category>()
                .Property(n => n.Title)
                .IsRequired()
                .HasMaxLength(75);

        }
    }
}
