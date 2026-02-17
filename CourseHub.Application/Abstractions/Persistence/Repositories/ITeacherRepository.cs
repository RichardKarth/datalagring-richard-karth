using CourseHub.Application.Teachers.PersistanceModels;


namespace CourseHub.Application.Abstractions.Persistence.Repositories
{
    public interface ITeacherRepository : IRepositoryBase<Teacher, int>
    {
        Task<bool> EmailExistsAsync(string email, CancellationToken ct);
    }
}
