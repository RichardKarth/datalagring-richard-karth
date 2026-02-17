using CourseHub.Application.Abstractions.Persistence;
using CourseHub.Application.Abstractions.Persistence.Repositories;
using CourseHub.Application.Students.PersistanceModels;
using CourseHub.Application.Teachers.PersistanceModels;
using CourseHub.Infrastructure.Data;
using CourseHub.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseHub.Infrastructure.Repositories
{
    public class TeacherEntityRepository(CHDbContext context) : RepositoryBase<TeacherEntity, int, Teacher>(context), ITeacherRepository
    {
        public override async Task AddAsync(Teacher model, CancellationToken ct = default)
        {
            if(model == null)
            {
                throw new ArgumentException("No teacher found.");
            }
            TeacherEntity entity = new TeacherEntity
            {
            Id = model.Id,
            FirstName = model.FirstName,
            LastName = model.LastName, 
            Email = model.Email
            };
            await Set.AddAsync(entity, ct);
        }

        public async Task<bool> EmailExistsAsync(string email, CancellationToken ct)
        {
            var normalized = email.Trim();
            return await Set.AsNoTracking().AnyAsync(x => x.Email == normalized, ct);
        }

        public override Teacher ToPersistanceModel(TeacherEntity entity)
        {
            Teacher teacherPersistanceModel = new Teacher
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email
            };
            return teacherPersistanceModel;
        }

        public async override Task UpdateAsync(Teacher model, CancellationToken ct = default)
        {
            var entity = await Set.SingleOrDefaultAsync(x => x.Id == model.Id, ct) ?? throw new ArgumentException($"Teacher with {model.Id} not found");

            entity.Email = model.Email.Trim();
            entity.FirstName = model.FirstName.Trim();
            entity.LastName = model.LastName.Trim();
        }
    }
}
