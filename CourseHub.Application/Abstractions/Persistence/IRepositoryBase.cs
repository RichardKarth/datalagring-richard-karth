
namespace CourseHub.Application.Abstractions.Persistence
{
    public interface IRepositoryBase<TModel, in TKey > where TModel : class where TKey : notnull
    {


        Task AddByIdAsync(TKey id, CancellationToken ct = default);

        Task<TModel> GetByIdAsync(CancellationToken ct = default);
        Task<IReadOnlyList<TModel>> ListAsync(CancellationToken ct = default);


        Task UpdateAsync(TModel model, CancellationToken ct = default);

        Task DeleteAsync(TModel model, CancellationToken ct = default);

    }
}
