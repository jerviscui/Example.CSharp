using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCoreTest;

public class BlogMapping : IEntityTypeConfiguration<Blog>
{

    #region IEntityTypeConfiguration implementations

    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Blog> builder)
    {
        builder.HasKey(o => o.Id);
        //*..1
        builder.HasMany(p => p.BlogTags).WithOne().HasForeignKey(o => o.BlogId).OnDelete(DeleteBehavior.Cascade);
    }

    #endregion

}

public class BlogTagMapping : IEntityTypeConfiguration<BlogTag>
{

    #region IEntityTypeConfiguration implementations

    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<BlogTag> builder)
    {
        builder.HasKey(o => o.Id);
    }

    #endregion

}
