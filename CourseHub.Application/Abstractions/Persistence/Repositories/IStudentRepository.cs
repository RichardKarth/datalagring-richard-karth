using CourseHub.Application.Students.PersistanceModels;


namespace CourseHub.Application.Abstractions.Persistence.Repositories;

public interface IStudentRepository : IRepositoryBase<Student, int>
{
    Task<bool> EmailExistsAsync(string email, CancellationToken ct = default);


}
