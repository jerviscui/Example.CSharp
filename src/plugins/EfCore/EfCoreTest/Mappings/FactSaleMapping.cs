using EfCoreTest.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCoreTest
{
    public class FactSaleMapping : IEntityTypeConfiguration<FactSale>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<FactSale> builder)
        {
            builder.HasNoKey();
            builder.HasIndex(o => o.DateId, "ci").IsClustered();
        }
    }
}
