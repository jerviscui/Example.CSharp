using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EfCoreTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Test();
            Console.WriteLine("Hello World!");
        }

        public static void Test()
        {
            //test 1 private set property
            //test 2 DeleteBehavior

            var builder = new DbContextOptionsBuilder<TestDbContext>();
            //builder.UseSqlServer(@"Server=.\sql2017;Initial Catalog=EfCoreTest;User ID=sa;Password=123456");
            builder.UseNpgsql(@"Host=localhost;Port=5432;Database=EfCoreTest;Username=postgres;Password=123456;");
            builder.UseLoggerFactory(LoggerFactory.Create(loggingBuilder => loggingBuilder.AddConsole()));
            builder.EnableSensitiveDataLogging();
            builder.EnableDetailedErrors();

            using var dbContext = new TestDbContext(builder.Options);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }
    }

    public class Person
    {
        public Person(long id, string name)
        {
            Id = id;
            Name = name;
        }

        public long Id { get; protected set; }

        public string Name { get; protected set; }

        public long? TeacherId { get; protected set; }

        public long FamilyId { get; protected set; }
    }

    public class Family
    {
        public Family(long id, string? address = null)
        {
            Id = id;
            Address = address;
        }

        public long Id { get; protected set; }

        [StringLength(200)]
        public string? Address { get; protected set; }
    }

    public class Teacher
    {
        public Teacher(long id, string name)
        {
            Id = id;
            Name = name;
        }

        public long Id { get; protected set; }

        [StringLength(100)]
        public string Name { get; protected set; }
    }

    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }

        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<Family> Families { get; set; }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var person = modelBuilder.Entity<Person>();
            person.HasKey(o => o.Id);
            person.Property(o => o.Id).ValueGeneratedNever();
            person.HasOne<Teacher>().WithMany().HasForeignKey(o => o.TeacherId).IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
            person.HasOne<Family>().WithMany().HasForeignKey(o => o.FamilyId).IsRequired()
                .OnDelete(DeleteBehavior.ClientNoAction);

            var teacher = modelBuilder.Entity<Teacher>();
            teacher.HasKey(o => o.Id);
            teacher.Property(o => o.Id).ValueGeneratedNever();

            var family = modelBuilder.Entity<Family>();
            family.HasKey(o => o.Id);
            family.Property(o => o.Id).ValueGeneratedNever();
        }
    }
}
