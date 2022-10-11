using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCoreTest
{
    public class PersonMapping : IEntityTypeConfiguration<Person>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).ValueGeneratedNever();
            builder.Property(o => o.Long).HasDefaultValue(0);
            builder.Property(o => o.Decimal).HasColumnType("decimal(18,2)").HasDefaultValue(0);
            //1..*
            builder.HasOne<Teacher>().WithMany().HasForeignKey(o => o.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne<Family>().WithMany().HasForeignKey(o => o.FamilyId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
