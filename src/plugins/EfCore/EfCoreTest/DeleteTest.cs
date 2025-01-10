using Microsoft.EntityFrameworkCore;

namespace EfCoreTest;

internal class DeleteTest : DbContextTest
{

    #region Constants & Statics

    public static async Task DeleteItems_FromPrimaryTable_OneToMany()
    {
        await using var dbContext = CreateSqliteMemoryDbContext();

        var blog = new Blog(20, "title1", "content1");
        AddIfNotExists(dbContext, blog);
        await dbContext.SaveChangesAsync();

        AddIfNotExists(dbContext, new BlogTag(21, blog.Id, "tag1"));
        AddIfNotExists(dbContext, new BlogTag(22, blog.Id, "tag2"));
        AddIfNotExists(dbContext, new BlogTag(23, blog.Id, "tag3"));
        AddIfNotExists(dbContext, new BlogTag(24, blog.Id, "tag4"));
        await dbContext.SaveChangesAsync();

        await dbContext.Entry(blog).Collection(o => o.BlogTags).LoadAsync();

        //remove all
        blog.RemoveTags();
        dbContext.SaveChanges(true);

        var tags = await dbContext.BlogTags.ToListAsync();
    }

    #endregion

}
