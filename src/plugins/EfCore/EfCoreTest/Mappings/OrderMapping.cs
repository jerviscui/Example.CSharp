using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCoreTest
{
    public class OrderMapping : IEntityTypeConfiguration<Order>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).ValueGeneratedNever();
            builder.OwnsOne(o => o.StreetAddress, navigationBuilder =>
            {
                navigationBuilder.HasIndex(o => o.City);
                navigationBuilder.HasIndex(o => o.Street);
            });
        }
    }
}
