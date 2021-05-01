using AutoPartsStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPartsStore.Persistence.Mapping
{
    public class ProductMapping: IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products")
                .HasKey(n => n.Id);
            builder.Property(n => n.Description)
                .IsRequired()
                .HasMaxLength(250);
            builder.Property(n => n.Title)
                .IsRequired()
                .HasMaxLength(75);
            builder.Property(n => n.Stock)
                .IsRequired()
                .HasDefaultValue(0);
            builder.Property(n => n.Price)
                .IsRequired();
            builder.Property(n => n.PublishDate)
                .IsRequired(false);
            builder.HasOne(n => n.Category)
                .WithMany(n => n.Products)
                .HasForeignKey(n => n.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(n => n.ProductCards)
                .WithOne(n => n.Product)
                .HasForeignKey(n=>n.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
