using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EfCoreTest
{
    internal class ExceptionTest : DbContextTest
    {
        public static async Task DbUpdateException_Retry_Test()
        {
            await using var dbContext = CreateMsSqlDbContext();

            //dbContext.Database.EnsureDeleted();
            //dbContext.Database.EnsureCreated();

            var blogTag = new BlogTag(1, "tag4");
            await dbContext.BlogTags.AddAsync(blogTag);

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                //e.Entries is 1
                foreach (var entry in e.Entries)
                {
                    entry.State = EntityState.Detached;
                }
            }

            var blog = new Blog("title1", "content1");
            await dbContext.Blogs.AddAsync(blog);
            await dbContext.SaveChangesAsync();

            await dbContext.BlogTags.AddRangeAsync(blogTag);

            await dbContext.SaveChangesAsync(); // save changes success
        }

        public static async Task DbUpdateException_CatchFirstEntry()
        {
            await using var dbContext = CreateMsSqlDbContext();

            //dbContext.Database.EnsureDeleted();
            //dbContext.Database.EnsureCreated();

            var blogTag = new BlogTag(1, "tag4");
            var blogTag2 = new BlogTag(2, "tag4");
            var blogTag3 = new BlogTag(3, "tag4");

            await dbContext.BlogTags.AddRangeAsync(blogTag, blogTag2, blogTag3);

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                //e.Entries length is 1, entry is blogTag
                foreach (var entry in e.Entries)
                {
                    entry.State = EntityState.Detached;
                }
            }

            var blog = new Blog("title1", "content1");
            await dbContext.Blogs.AddAsync(blog);
            await dbContext.SaveChangesAsync();

            await dbContext.BlogTags.AddRangeAsync(blogTag, blogTag2, blogTag3);

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch
            {
                //will throw DbUpdateException with blogTag2
            }
        }
    }
}
