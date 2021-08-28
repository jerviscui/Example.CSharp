using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace EfCoreTest
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; } = null!;

        public DbSet<Teacher> Teachers { get; set; } = null!;

        public DbSet<Family> Families { get; set; } = null!;

        public DbSet<Order> Orders { get; set; } = null!;

        public DbSet<Post> Posts { get; set; } = null!;

        public DbSet<Tag> Tags { get; set; } = null!;

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TestDbContext).GetTypeInfo().Assembly);
            //modelBuilder.Entity<Person>().Navigation()
            CreateSeed(modelBuilder);
        }

        private static void CreateSeed(ModelBuilder modelBuilder)
        {
            var family = new Family(1, "address");
            var family2 = new Family(2, "address", family.Id);
            var family3 = new Family(3, "address", family2.Id);
            modelBuilder.Entity<Family>().HasData(family, family2, family3);

            var teacher = new Teacher(2, "teacher");
            modelBuilder.Entity<Teacher>().HasData(teacher);

            var person = new Person(1, "name", family.Id, null);
            var person2 = new Person(2, "name", family2.Id, teacher.Id);
            var person3 = new Person(3, "name", family3.Id, teacher.Id);
            modelBuilder.Entity<Person>().HasData(person, person2, person3);

            for (int i = 0; i < 100; i++)
            {
                modelBuilder.Entity<Person>().HasData(new Person(100 + i, $"name{i}", family2.Id, null));
            }

            //todo: https://github.com/dotnet/efcore/issues/25586
            //var order = new Order(1, "buyer", "street", "city");
            //modelBuilder.Entity<Order>().HasData(order);

            var post1 = new Post { PostId = 1, Content = "content1", Title = "title1" };
            var post2 = new Post { PostId = 2, Content = "content2", Title = "title2" };
            modelBuilder.Entity<Post>().HasData(post1, post2);

            var tag1 = new Tag { TagId = "tag1" /*, Posts = new List<Post> { post1, post2 }*/ };
            var tag2 = new Tag { TagId = "tag2" /*, Posts = new List<Post> { post1, post2 }*/ };
            modelBuilder.Entity<Tag>().HasData(tag1, tag2);

            modelBuilder.SharedTypeEntity<Dictionary<string, object>>("PostTag")
                .HasData(new { post1.PostId, tag1.TagId }, new { post1.PostId, tag2.TagId },
                    new { post2.PostId, tag1.TagId }, new { post2.PostId, tag2.TagId });
        }
    }
}
