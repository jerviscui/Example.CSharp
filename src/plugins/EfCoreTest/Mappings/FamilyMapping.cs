using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCoreTest
{
    public class FamilyMapping : IEntityTypeConfiguration<Family>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<Family> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).ValueGeneratedNever();
            builder.HasOne<Family>().WithMany().HasForeignKey(o => o.OldFamilyId); //.IsRequired(false); 可以省略，使用属性类型定义
        }
    }
}
