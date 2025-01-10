using EfCoreTest.Paging;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EfCoreTest;

public class TestDbContext : DbContext
{

    #region Constants & Statics

    private static void CreateSeed(ModelBuilder modelBuilder)
    {
        var family = new Family(1, "address");
        var family2 = new Family(2, "address", family.Id);
        var family3 = new Family(3, "address", family2.Id);
        _ = modelBuilder.Entity<Family>().HasData(family, family2, family3);

        var teacher = new Teacher(2, "teacher");
        _ = modelBuilder.Entity<Teacher>().HasData(teacher);

        var person = new Person(1, "name", family.Id, null);
        var person2 = new Person(2, "name", family2.Id, teacher.Id);
        var person3 = new Person(3, "name", family3.Id, teacher.Id);
        _ = modelBuilder.Entity<Person>().HasData(person, person2, person3);

        for (var i = 0; i < 100; i++)
        {
            _ = modelBuilder.Entity<Person>().HasData(new Person(100 + i, $"name{i}", family2.Id, null));
        }

        //todo: Data Seeding not support navigations https://github.com/dotnet/efcore/issues/25586
        //var order = new Order(1, "buyer", "street", "city");
        //modelBuilder.Entity<Order>().HasData(order);

        var post1 = new Post { PostId = 1, Content = "content1", Title = "title1" };
        var post2 = new Post { PostId = 2, Content = "content2", Title = "title2" };
        _ = modelBuilder.Entity<Post>().HasData(post1, post2);

        var tag1 = new Tag { TagId = "tag1" /*, Posts = new List<Post> { post1, post2 }*/ };
        var tag2 = new Tag { TagId = "tag2" /*, Posts = new List<Post> { post1, post2 }*/ };
        _ = modelBuilder.Entity<Tag>().HasData(tag1, tag2);

        _ = modelBuilder.SharedTypeEntity<Dictionary<string, object>>("PostTag")
            .HasData(
                new { post1.PostId, tag1.TagId },
                new { post1.PostId, tag2.TagId },
                new { post2.PostId, tag1.TagId },
                new { post2.PostId, tag2.TagId });

        var blog = new Blog(1, "aaaa", "bbbb");
        _ = modelBuilder.Entity<Blog>().HasData(blog);

        var blogTag1 = new BlogTag(1, 1, "tag1");
        var blogTag2 = new BlogTag(2, 1, "tag2");
        _ = modelBuilder.Entity<BlogTag>().HasData(blogTag1, blogTag2);
    }

    #endregion

    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {
    }

    #region Properties

    public DbSet<Blog> Blogs { get; set; } = null!;

    public DbSet<BlogTag> BlogTags { get; set; } = null!;

    public DbSet<DetailedSplitOrder> DetailedSplitOrders { get; set; } = null!;

    public DbSet<FactSale> FactSales { get; set; } = null!;

    public DbSet<Family> Families { get; set; } = null!;

    public DbSet<MssqlRowVersion> MssqlRowVersions { get; set; } = null!;

    public DbSet<MysqlRowVersion> MysqlRowVersions { get; set; } = null!;

    public DbSet<Order> Orders { get; set; } = null!;

    public DbSet<Person> Persons { get; set; } = null!;

    public DbSet<PgsqlRowVersion> PgsqlRowVersions { get; set; } = null!;

    public DbSet<Post> Posts { get; set; } = null!;

    public DbSet<SplitOrder> SplitOrders { get; set; } = null!;

    public DbSet<Tag> Tags { get; set; } = null!;

    public DbSet<Teacher> Teachers { get; set; } = null!;

    #endregion

    #region Methods

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        SetDatabaseProvider(modelBuilder);

        _ = modelBuilder.ApplyConfigurationsFromAssembly(typeof(TestDbContext).GetTypeInfo().Assembly);

        CreateSeed(modelBuilder);
    }

    public int SaveChanges(bool softDelete, bool acceptAllChangesOnSuccess = true)
    {
        if (softDelete)
        {
            foreach (var entry in ChangeTracker.Entries<ISoftDelete>())
            {
                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDelete = true;
                }
            }
        }

        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    #endregion

    #region DatabaseProvider

    public enum EfCoreDatabaseProvider
    {
        SqlServer,

        MySql,

        Oracle,

        PostgreSql,

        Sqlite,

        InMemory,

        Cosmos,

        Firebird
    }

    public const string ProviderKey = "ProviderKey";

    private void SetDatabaseProvider(ModelBuilder modelBuilder)
    {
        var provider = GetDatabaseProviderOrNull(modelBuilder);
        if (provider != null)
        {
            modelBuilder.Model.SetAnnotation(ProviderKey, provider);
        }
    }

    protected virtual EfCoreDatabaseProvider? GetDatabaseProviderOrNull(ModelBuilder modelBuilder)
    {
        return Database.ProviderName switch
        {
            "Microsoft.EntityFrameworkCore.SqlServer" => (EfCoreDatabaseProvider?)EfCoreDatabaseProvider.SqlServer,
            "Npgsql.EntityFrameworkCore.PostgreSQL" => (EfCoreDatabaseProvider?)EfCoreDatabaseProvider.PostgreSql,
            "Pomelo.EntityFrameworkCore.MySql" => (EfCoreDatabaseProvider?)EfCoreDatabaseProvider.MySql,
            "Oracle.EntityFrameworkCore" or "Devart.Data.Oracle.Entity.EFCore" => (EfCoreDatabaseProvider?)EfCoreDatabaseProvider.Oracle,
            "Microsoft.EntityFrameworkCore.Sqlite" => (EfCoreDatabaseProvider?)EfCoreDatabaseProvider.Sqlite,
            "Microsoft.EntityFrameworkCore.InMemory" => (EfCoreDatabaseProvider?)EfCoreDatabaseProvider.InMemory,
            "FirebirdSql.EntityFrameworkCore.Firebird" => (EfCoreDatabaseProvider?)EfCoreDatabaseProvider.Firebird,
            "Microsoft.EntityFrameworkCore.Cosmos" => (EfCoreDatabaseProvider?)EfCoreDatabaseProvider.Cosmos,
            _ => null,
        };
    }

    #endregion

}
