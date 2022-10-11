using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCoreTest
{
    public class SplitOrderMapping : IEntityTypeConfiguration<SplitOrder>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<SplitOrder> builder)
        {
            builder.ToTable(nameof(SplitOrder));

            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).ValueGeneratedNever();
            builder.Property(o => o.Status).HasColumnName(nameof(SplitOrder.Status)); //此配置不能少

            builder.HasOne(o => o.DetailedSplitOrder).WithOne().HasForeignKey<DetailedSplitOrder>(o => o.Id);
        }
    }

    public class DetailedSplitOrderMapping : IEntityTypeConfiguration<DetailedSplitOrder>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<DetailedSplitOrder> builder)
        {
            builder.ToTable(nameof(SplitOrder));

            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).ValueGeneratedNever();
            builder.Property(o => o.Status).HasColumnName(nameof(SplitOrder.Status)); //此配置不能少
        }
    }
}
