using CourseHub.Application.Abstractions.Persistence.Repositories;
using CourseHub.Infrastructure.Data;
using CourseHub.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;


namespace CourseHub.Infrastructure.Repositories;

public class EnrollmentRepository(CHDbContext context) : IEnrollmentRepository
{

    private DbSet<EnrollmentEntity> Set => context.Set<EnrollmentEntity>();

    public async Task<bool> ExistsAsync(int studentId, int courseInstanceId, CancellationToken ct = default)
    {
        return await Set.AsNoTracking()
            .AnyAsync(x => x.StudentId == studentId && x.CourseInstanceId == courseInstanceId, ct);
    }

    public async Task AddAsync(int studentId, int courseInstanceId, CancellationToken ct = default)
    {
        var entity = new EnrollmentEntity
        {
            StudentId = studentId,
            CourseInstanceId = courseInstanceId
        };

        await Set.AddAsync(entity, ct);
    }

    public async Task DeleteAsync(int studentId, int courseInstanceId, CancellationToken ct = default)
    {
        var entity = await Set.SingleOrDefaultAsync(
            x => x.StudentId == studentId && x.CourseInstanceId == courseInstanceId, ct);

        if (entity is null) return;

        Set.Remove(entity);
    }

    public async Task<int> CountByCourseInstanceAsync(int courseInstanceId, CancellationToken ct = default)
    {
        return await Set.AsNoTracking()
            .CountAsync(x => x.CourseInstanceId == courseInstanceId, ct);
    }

    //visar vilka studenter som går på en särskilld kurs
    public async Task<IReadOnlyList<int>> ListStudentIdsByCourseInstanceAsync(int courseInstanceId, CancellationToken ct = default)
    {
        return await Set.AsNoTracking()
            .Where(x => x.CourseInstanceId == courseInstanceId)
            .Select(x => x.StudentId)
            .ToListAsync(ct);
    }

    //kollar vilka kurs tillfällen en särskilld student går på
    public async Task<IReadOnlyList<int>> ListCourseInstanceIdsByStudentAsync(int studentId, CancellationToken ct = default)
    {
        return await Set.AsNoTracking()
            .Where(x => x.StudentId == studentId)
            .Select(x => x.CourseInstanceId)
            .ToListAsync(ct);
    }
}
