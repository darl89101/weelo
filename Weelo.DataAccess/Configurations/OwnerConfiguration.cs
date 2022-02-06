using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Weelo.Domain.Entities;

namespace Weelo.DataAccess.Configurations
{
    internal class OwnerConfiguration : IEntityTypeConfiguration<Owner>
    {
        public void Configure(EntityTypeBuilder<Owner> builder)
        {
            builder.Property(m => m.Id)
                .HasColumnName("IdOwner");
            builder.Property(m => m.DocumentNumber)
                .IsUnicode(false)
                .HasMaxLength(20)
                .IsRequired();
            builder.Property(m => m.Name)
                .IsUnicode(false)
                .HasMaxLength(250)
                .IsRequired();
            builder.Property(m => m.Address)
                .IsUnicode(false)
                .HasMaxLength(250)
                .IsRequired();

            builder.HasIndex(m => m.DocumentNumber).IsUnique();
        }
    }
}
