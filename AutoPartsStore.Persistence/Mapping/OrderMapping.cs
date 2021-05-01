using AutoPartsStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPartsStore.Persistence.Mapping
{
    public class OrderMapping : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders")
                .HasKey(n => n.Id);
            builder.Property(n => n.Address)
                .IsRequired()
                .HasMaxLength(350);
            builder.Property(n => n.PostalCode)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(n => n.TotalPrice)
                .IsRequired();
            builder.HasOne(n => n.User)
                .WithMany(n => n.Orders)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(n => n.ProductCards)
                .WithOne(n => n.Order)
                .HasForeignKey(n => n.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
