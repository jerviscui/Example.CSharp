using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCoreTest
{
    public class MssqlRowVersionMapping : IEntityTypeConfiguration<MssqlRowVersion>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<MssqlRowVersion> builder)
        {
            builder.HasKey(o => o.Id);
            if ((TestDbContext.EfCoreDatabaseProvider)builder.Metadata.Model[TestDbContext.ProviderKey] ==
                TestDbContext.EfCoreDatabaseProvider.SqlServer)
            {
                builder.Property(o => o.RowVersion).IsRowVersion();
            }
        }
    }

    public class MysqlRowVersionMapping : IEntityTypeConfiguration<MysqlRowVersion>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<MysqlRowVersion> builder)
        {
            builder.HasKey(o => o.Id);
            if ((TestDbContext.EfCoreDatabaseProvider)builder.Metadata.Model[TestDbContext.ProviderKey] ==
                TestDbContext.EfCoreDatabaseProvider.MySql)
            {
                builder.Property(o => o.RowVersion).IsConcurrencyToken();
            }
        }
    }

    public class PgsqlRowVersionMapping : IEntityTypeConfiguration<PgsqlRowVersion>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<PgsqlRowVersion> builder)
        {
            builder.HasKey(o => o.Id);

            if ((TestDbContext.EfCoreDatabaseProvider)builder.Metadata.Model[TestDbContext.ProviderKey] ==
                TestDbContext.EfCoreDatabaseProvider.PostgreSql)
            {
                builder.UseXminAsConcurrencyToken();
            }
        }
    }
}
