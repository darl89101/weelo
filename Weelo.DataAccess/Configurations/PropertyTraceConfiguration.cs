using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Weelo.Domain.Entities;

namespace Weelo.DataAccess.Configurations
{
    internal class PropertyTraceConfiguration : IEntityTypeConfiguration<PropertyTrace>
    {
        public void Configure(EntityTypeBuilder<PropertyTrace> builder)
        {
            builder.Property(m => m.Id)
                .HasColumnName("IdPropertyTrace");
            builder.Property(m => m.Name)
                .IsUnicode(false)
                .HasMaxLength(250)
                .IsRequired();
            builder.Property(m => m.Value)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            builder.Property(m => m.Tax)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.HasOne(m => m.Property)
                .WithMany(m => m.PropertyTraces)
                .HasForeignKey(m => m.IdProperty)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
