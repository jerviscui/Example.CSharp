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

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var person = modelBuilder.Entity<Person>();
            person.HasKey(o => o.Id);
            person.Property(o => o.Id).ValueGeneratedNever();
            person.Property(o => o.Long).HasDefaultValue(0);
            person.Property(o => o.Decimal).HasColumnType("decimal(18,2)").HasDefaultValue(0);
            person.HasOne<Teacher>().WithMany().HasForeignKey(o => o.TeacherId).IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull);
            person.HasOne<Family>().WithMany().HasForeignKey(o => o.FamilyId).IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            var teacher = modelBuilder.Entity<Teacher>();
            teacher.HasKey(o => o.Id);
            teacher.Property(o => o.Id).ValueGeneratedNever();

            var family = modelBuilder.Entity<Family>();
            family.HasKey(o => o.Id);
            family.Property(o => o.Id).ValueGeneratedNever();

            CreateSeed(modelBuilder);
        }

        private static void CreateSeed(ModelBuilder modelBuilder)
        {
            //var family = new Family(1, "address");
            //var person = new Person(1, "name", family.Id, null);

            //modelBuilder.Entity<Family>().HasData(family);
            //modelBuilder.Entity<Person>().HasData(person);
        }
    }
}
