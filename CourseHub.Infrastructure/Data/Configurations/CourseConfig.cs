using CourseHub.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseHub.Infrastructure.Data.Configurations;

public class CourseConfig : IEntityTypeConfiguration<CourseEntity>
{
    public void Configure(EntityTypeBuilder<CourseEntity> entity)
    {
        entity.ToTable("Courses");

        entity.HasKey(x => x.Id).HasName("PK_Courses_Id");
        entity.Property(x => x.Id).ValueGeneratedOnAdd();

        entity.Property(x => x.Title).IsRequired().HasMaxLength(200);
        entity.Property(x => x.Description).HasMaxLength(2000);

        entity.Property(x => x.DurationDays).IsRequired();
    }
}
