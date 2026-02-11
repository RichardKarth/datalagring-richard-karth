
using CourseHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseHub.Infrastructure.Data
{
    public class CHDbContext(DbContextOptions<CHDbContext> options) : DbContext(options)
    {

        public DbSet<CourseEntity> Courses => Set<CourseEntity>();
        public DbSet<CourseInstanceEntity> CourseInstances => Set<CourseInstanceEntity>();
        public DbSet<TeacherEntity> Teachers => Set<TeacherEntity>();
        public DbSet<StudentEntity> Students => Set<StudentEntity>();
        public DbSet<EnrollmentEntity> Enrollments => Set<EnrollmentEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CHDbContext).Assembly);
        }
    }
}
