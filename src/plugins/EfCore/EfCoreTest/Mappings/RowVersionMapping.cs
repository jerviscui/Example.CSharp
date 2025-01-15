using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCoreTest;

public class MssqlRowVersionMapping : IEntityTypeConfiguration<MssqlRowVersion>
{

    #region IEntityTypeConfiguration implementations

    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<MssqlRowVersion> builder)
    {
        builder.HasKey(o => o.Id);
        if ((TestDbContext.EfCoreDatabaseProvider)builder.Metadata.Model[TestDbContext.ProviderKey]!
            == TestDbContext.EfCoreDatabaseProvider.SqlServer)
        {
            builder.Property(o => o.RowVersion).IsRowVersion();
        }
    }

    #endregion

}

public class MysqlRowVersionMapping : IEntityTypeConfiguration<MysqlRowVersion>
{

    #region IEntityTypeConfiguration implementations

    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<MysqlRowVersion> builder)
    {
        builder.HasKey(o => o.Id);
        if ((TestDbContext.EfCoreDatabaseProvider)builder.Metadata.Model[TestDbContext.ProviderKey]!
            == TestDbContext.EfCoreDatabaseProvider.MySql)
        {
            builder.Property(o => o.RowVersion).IsConcurrencyToken();
        }
    }

    #endregion

}

public class PgsqlRowVersionMapping : IEntityTypeConfiguration<PgsqlRowVersion>
{

    #region IEntityTypeConfiguration implementations

    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<PgsqlRowVersion> builder)
    {
        builder.HasKey(o => o.Id);

        if ((TestDbContext.EfCoreDatabaseProvider)builder.Metadata.Model[TestDbContext.ProviderKey]!
            == TestDbContext.EfCoreDatabaseProvider.PostgreSql)
        {
            builder.Property(o => o.Version).IsRowVersion();
        }
    }

    #endregion

}
