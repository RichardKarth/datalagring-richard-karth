
namespace CourseHub.Application.Abstractions.Persistence
{
    public interface IRepositoryBase<TModel, in TKey >
    {


        Task AddByIdAsync(TKey id, CancellationToken ct = default);

        Task<TModel?> GetByIdAsync(TKey id, CancellationToken ct = default);
        Task<IReadOnlyList<TModel>> ListAsync(CancellationToken ct = default);


        Task UpdateAsync(TModel model, CancellationToken ct = default);

        Task DeleteByIdAsync(TKey id, CancellationToken ct = default);

    }
}
