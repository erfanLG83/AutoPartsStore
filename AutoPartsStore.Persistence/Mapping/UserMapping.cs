using AutoPartsStore.Domain.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPartsStore.Persistence.Mapping
{
    public class UserMapping : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(n => n.Address)
                .IsRequired(false)
                .HasMaxLength(300);
            builder.Property(n => n.ImageName)
                .IsRequired(false)
                .IsUnicode(false);
            builder.Property(n => n.LastName)
                .IsRequired(false)
                .HasMaxLength(75);
            builder.Property(n => n.FirstName)
                .IsRequired()
                .HasMaxLength(75);
            builder.Property(n => n.PostalCode)
                .IsRequired(false);
        }
    }
}
