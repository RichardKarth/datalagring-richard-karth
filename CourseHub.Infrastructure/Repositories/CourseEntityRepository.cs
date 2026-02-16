using CourseHub.Application.Abstractions.Persistence.Repositories;
using CourseHub.Application.Courses.PersistanceModels;
using CourseHub.Infrastructure.Data;
using CourseHub.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseHub.Infrastructure.Repositories;

public sealed class CourseEntityRepository(CHDbContext context)
    : RepositoryBase<CourseEntity, int, Course>(context), ICourseRepository
{
    public override async Task AddAsync(Course model, CancellationToken ct = default)
    {
        if (model is null)
            throw new ArgumentNullException(nameof(model));

        var entity = new CourseEntity
        {
            // Id sätts av databasen
            Title = model.Title,
            Description = model.Description,
            DurationDays = model.DurationDays
        };

        await Set.AddAsync(entity, ct);
    }

    public override Course ToPersistanceModel(CourseEntity entity)
    {
        return new Course
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            DurationDays = entity.DurationDays
        };
    }

    public override async Task UpdateAsync(Course model, CancellationToken ct = default)
    {
        var entity = await Set.SingleOrDefaultAsync(x => x.Id == model.Id, ct)
            ?? throw new ArgumentException($"Course with id {model.Id} not found");

        entity.Title = model.Title.Trim();
        entity.Description = model.Description?.Trim();
        entity.DurationDays = model.DurationDays;
    }
}
