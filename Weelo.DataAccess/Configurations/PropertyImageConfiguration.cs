using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Weelo.Domain.Entities;

namespace Weelo.DataAccess.Configurations
{
    internal class PropertyImageConfiguration : IEntityTypeConfiguration<PropertyImage>
    {
        public void Configure(EntityTypeBuilder<PropertyImage> builder)
        {
            builder.Property(m => m.Id)
                .HasColumnName("IdPropertyImage");

            builder.HasOne(m => m.Property)
                .WithMany(m => m.PropertyImages)
                .HasForeignKey(m => m.IdProperty)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
