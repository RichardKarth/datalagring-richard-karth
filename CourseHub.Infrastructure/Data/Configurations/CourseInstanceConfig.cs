using CourseHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace CourseHub.Infrastructure.Data.Configurations;

internal class CourseInstanceConfig : IEntityTypeConfiguration<CourseInstanceEntity>
{
    public void Configure(EntityTypeBuilder<CourseInstanceEntity> entity)
    {
        entity.ToTable("CourseInstances");

        entity.HasKey(x => x.Id).HasName("PK_CourseInstances_Id");
        entity.Property(x => x.Id).ValueGeneratedOnAdd();

        entity.Property(x => x.StartDateUtc).IsRequired();
        entity.Property(x => x.EndDateUtc).IsRequired();
        entity.Property(x => x.Location).IsRequired().HasMaxLength(200);
        entity.Property(x => x.Capacity).IsRequired();

        entity.HasOne(x => x.Course)
              .WithMany(c => c.CourseInstances)
              .HasForeignKey(x => x.CourseId)
              .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(x => x.Teacher)
              .WithMany(t => t.CourseInstances)
              .HasForeignKey(x => x.TeacherId)
              .OnDelete(DeleteBehavior.Restrict);
    }
}

