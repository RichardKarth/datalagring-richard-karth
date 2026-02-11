using CourseHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseHub.Infrastructure.Data.Configurations;

internal class StudentConfig : IEntityTypeConfiguration<StudentEntity>
{
    public void Configure(EntityTypeBuilder<StudentEntity> entity)
    {
        entity.ToTable("Students", tb =>
        {
            tb.HasCheckConstraint("CK_Student_Email_NotEmpty", "LTRIM(RTRIM([Email])) <> ''");
        });

        entity.HasKey(x => x.Id).HasName("PK_Students_Id");

        entity.Property(x => x.Id).ValueGeneratedOnAdd();

        entity.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
        entity.Property(x => x.LastName).IsRequired().HasMaxLength(100);
        entity.Property(x => x.Email).IsRequired().HasMaxLength(256);
        entity.Property(x => x.PhoneNumber).HasMaxLength(13).IsUnicode(false);

        entity.HasIndex(x => x.Email).IsUnique().HasDatabaseName("UQ_Students_Email");
    }
}
