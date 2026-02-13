using CourseHub.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseHub.Infrastructure.Data.Configurations;

public class TeacherConfig : IEntityTypeConfiguration<TeacherEntity>
{
    public void Configure(EntityTypeBuilder<TeacherEntity> entity)
    {
        entity.ToTable("Teachers");


        entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
        entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
        entity.Property(e => e.Email).IsRequired().HasMaxLength(100);

        entity.Property(e => e.Id).ValueGeneratedOnAdd();
        entity.HasKey(e => e.Id).HasName("PK_Teachers_Id");

        entity.HasIndex(e => e.Email).IsUnique().HasDatabaseName("UQ_Teachers_Email");




    }
}
