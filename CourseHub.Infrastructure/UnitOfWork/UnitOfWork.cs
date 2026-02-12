
using CourseHub.Application.Abstractions.Persistence;
using CourseHub.Infrastructure.Data;

namespace CourseHub.Infrastructure.UnitOfWork;

public class UnitOfWork(CHDbContext context) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken ct = default) => context.SaveChangesAsync(ct);
}
