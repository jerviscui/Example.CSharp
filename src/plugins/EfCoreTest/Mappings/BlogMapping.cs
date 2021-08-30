using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCoreTest
{
    public class BlogMapping : IEntityTypeConfiguration<Blog>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.HasKey(o => o.Id);
            builder.HasMany(p => p.BlogTags).WithOne().HasForeignKey(o => o.BlogId).OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class BlogTaggMapping : IEntityTypeConfiguration<BlogTag>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<BlogTag> builder)
        {
            builder.HasKey(o => o.Id);
        }
    }
}
