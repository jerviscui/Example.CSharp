using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCoreTest
{
    public class TeacherMapping : IEntityTypeConfiguration<Teacher>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).ValueGeneratedNever();
        }
    }
}
