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
            family.HasOne<Family>().WithMany().HasForeignKey(o => o.OldFamilyId).IsRequired(false);

            var order = modelBuilder.Entity<Order>();
            order.HasKey(o => o.Id);
            order.Property(o => o.Id).ValueGeneratedNever();
            order.OwnsOne(o => o.StreetAddress, builder =>
            {
                builder.HasIndex(o => o.City);
                builder.HasIndex(o => o.Street);
            });

            //CreateSeed(modelBuilder);
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

            for (int i = 0; i < 10000; i++)
            {
                modelBuilder.Entity<Person>().HasData(new Person(100 + i, $"name{i}", family2.Id, null));
            }

            //todo: https://github.com/dotnet/efcore/issues/25586
            //var order = new Order(1, "buyer", "street", "city");
            //modelBuilder.Entity<Order>().HasData(order);
        }
    }
}
