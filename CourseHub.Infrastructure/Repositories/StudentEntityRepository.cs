

using CourseHub.Application.Abstractions.Persistence;
using CourseHub.Application.Abstractions.Persistence.Repositories;
using CourseHub.Application.Students.PersistanceModels;
using CourseHub.Infrastructure.Data;
using CourseHub.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace CourseHub.Infrastructure.Repositories;

public class StudentEntityRepository(CHDbContext context) : RepositoryBase<StudentEntity, int, Student>(context), IStudentRepository
{
    public override async Task AddAsync(Student model, CancellationToken ct = default)
    {
        if(model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }
        StudentEntity entity = new StudentEntity
        {

            //jag lämnar id och concurrency unset eftersom att de sätts av databasen

            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            CreatedAtUtc = model.CreatedAtUtc == default ? DateTime.UtcNow : model.CreatedAtUtc,
            ModifiedAtUtc = model.ModifiedAtUtc == default ? DateTime.UtcNow : model.ModifiedAtUtc,

        };
        await Set.AddAsync(entity, ct);
    }

    public override Student ToPersistanceModel(StudentEntity entity)
    {
        return new Student(
            entity.Id,
            entity.Email,
            entity.FirstName,
            entity.LastName,
            entity.PhoneNumber,
            entity.CreatedAtUtc,
            entity.ModifiedAtUtc,
            entity.Concurrency
        );
    }

    public override async Task UpdateAsync(Student model, CancellationToken ct = default)
    {
        var entity = await Set.SingleOrDefaultAsync(x => x.Id == model.Id, ct) ?? throw new ArgumentException($"User with {model.Id} not found");
        Context.Entry(entity).Property(x => x.Concurrency).OriginalValue = model.RowVersion;

        entity.Email = model.Email.Trim();
        entity.FirstName = model.FirstName.Trim();
        entity.LastName = model.LastName.Trim();
        entity.PhoneNumber = model.PhoneNumber;
        entity.PhoneNumber = string.IsNullOrWhiteSpace(model.PhoneNumber) ? null : model.PhoneNumber.Trim();
        entity.ModifiedAtUtc = DateTime.UtcNow;


    }
    public async Task<bool> EmailExistsAsync(string email, CancellationToken ct = default)
    {
        var normalized = email.Trim();
        return await Set.AsNoTracking().AnyAsync(x => x.Email == normalized, ct);
    }

}



