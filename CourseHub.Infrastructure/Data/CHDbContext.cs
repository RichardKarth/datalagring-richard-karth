
using CourseHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseHub.Infrastructure.Data
{
    public class CHDbContext(DbContextOptions<CHDbContext> options) : DbContext(options)
    {

        public DbSet<StudentEntity> Student => Set<StudentEntity> ();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentEntity>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Students_Id");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();



                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);

                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);

                entity.Property(e => e.Email).IsRequired().HasMaxLength(256);

                entity.Property(e => e.PhoneNumber).HasMaxLength(13).IsUnicode(false).IsRequired(false);

                entity.Property(e => e.Concurrency).IsRowVersion().IsConcurrencyToken().IsRequired();

                entity.Property(e => e.CreatedAtUtc).HasPrecision(0).HasDefaultValueSql("(SYSUTCDATETIME())", "DF_Students_CreatedAtUtc");
                entity.Property(e => e.ModifiedAtUtc).HasPrecision(0).HasDefaultValueSql("(SYSUTCDATETIME())", "DF_Students_ModifiedAtUtc").ValueGeneratedOnAddOrUpdate();


                entity.HasIndex(e => e.Email, "UQ_Students_Email").IsUnique();
                entity.ToTable(tb => tb.HasCheckConstraint("CK_Student_Email_NotEmpty", "LTRIM(RTRIM([Email])) <> ''"));
            });
        }
    }
}
