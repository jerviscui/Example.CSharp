using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EfCoreTest
{
    internal class DeleteTest : DbContextTest
    {
        public static async Task DeleteItems_FromPrimaryTable_OneToMany()
        {
            await using var dbContext = CreateSqliteMemoryDbContext();

            var blog = new Blog("title1", "content1");
            AddIfNotExists(dbContext, blog);
            await dbContext.SaveChangesAsync();

            AddIfNotExists(dbContext, new BlogTag(blog.Id, "tag1"));
            AddIfNotExists(dbContext, new BlogTag(blog.Id, "tag2"));
            AddIfNotExists(dbContext, new BlogTag(blog.Id, "tag3"));
            AddIfNotExists(dbContext, new BlogTag(blog.Id, "tag4"));
            await dbContext.SaveChangesAsync();

            await dbContext.Entry(blog).Collection(o => o.BlogTags).LoadAsync();

            //remove all
            blog.BlogTags.RemoveAll(_ => true);
            dbContext.SaveChanges(true);

            var tags = await dbContext.BlogTags.ToListAsync();
        }
    }
}
