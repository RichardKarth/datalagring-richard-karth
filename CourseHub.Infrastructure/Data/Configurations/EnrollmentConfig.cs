using CourseHub.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace CourseHub.Infrastructure.Data.Configurations
{
    public class EnrollmentConfig : IEntityTypeConfiguration<EnrollmentEntity>
    {
        public void Configure(EntityTypeBuilder<EnrollmentEntity> entity)
        {
            entity.ToTable("Enrollments");


            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasKey(e => e.Id).HasName("PK_Enrollments_Id");

            entity.HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Enrollments_Students_StudentId");

            entity.HasOne(e => e.CourseInstance)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseInstanceId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Enrollments_CourseInstances_CourseInstanceId");

            entity.HasIndex(x => new { x.StudentId, x.CourseInstanceId }).IsUnique().HasDatabaseName("UQ_Enrollments_Student_CourseInsance");
        }
    }
}
