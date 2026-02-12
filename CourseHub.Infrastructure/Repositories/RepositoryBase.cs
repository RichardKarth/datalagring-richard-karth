using CourseHub.Application.Abstractions.Persistence;
using CourseHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CourseHub.Infrastructure.Repositories;

public abstract class RepositoryBase<TModel, TKey, TEntity>(CHDbContext context) : IRepositoryBase<TModel, TKey>
    where TModel : class
    where TEntity : class, 
    IEntity<TKey>
{

    protected CHDbContext Context { get; } = context;
    protected DbSet<TEntity> Set => Context.Set<TEntity>();




    public abstract TModel ToModel(TEntity entity);
    public abstract Task AddByIdAsync(TKey id, CancellationToken ct = default);
    public abstract Task UpdateAsync(TModel model, CancellationToken ct = default);



    public virtual async Task DeleteByIdAsync(TKey id, CancellationToken ct = default)
    {
        var entity = await Set.SingleOrDefaultAsync(x => x.Id!.Equals (id), ct);
        if (entity == null) return;

        Set.Remove(entity);
    }

    public virtual async Task<TModel?> GetByIdAsync(TKey id, CancellationToken ct = default)
    {
        var entity = await Set.AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id!.Equals(id), ct);

        if (entity == null)
            return default;

        return ToModel(entity);
    }

    public virtual async Task<IReadOnlyList<TModel>> ListAsync(CancellationToken ct = default)
    {
        var entities = await Set.AsNoTracking().ToListAsync(ct);

        return entities.Select(ToModel).ToList();
    }
}
