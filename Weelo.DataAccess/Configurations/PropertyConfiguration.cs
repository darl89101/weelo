using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Weelo.Domain.Entities;

namespace Weelo.DataAccess.Configurations
{
    internal class PropertyConfiguration : IEntityTypeConfiguration<Property>
    {
        public void Configure(EntityTypeBuilder<Property> builder)
        {
            builder.Property(m => m.Id)
                .HasColumnName("IdProperty");
            builder.Property(m => m.Name)
                .IsUnicode(false)
                .HasMaxLength(250)
                .IsRequired();
            builder.Property(m => m.Address)
                .IsUnicode(false)
                .HasMaxLength(250)
                .IsRequired();
            builder.Property(m => m.CodeInternal)
                .IsUnicode(false)
                .HasMaxLength(36)
                .IsRequired();
            builder.Property(m => m.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.HasOne(m => m.Owner)
                .WithMany(m => m.Properties)
                .HasForeignKey(m => m.IdOwner)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
