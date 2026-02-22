

using CourseHub.Application.Abstractions.Persistence.Repositories;
using CourseHub.Application.Students.Inputs;
using CourseHub.Application.Students.Outputs;

namespace CourseHub.Application.Abstractions.Services;

public interface IStudentService
{
    Task CreateAsync(CreateStudentInput input, CancellationToken ct);

    Task<StudentProfileOutput?> GetByIdAsync(int id, CancellationToken ct);
    Task<IReadOnlyList<StudentProfileOutput>> GetAllAsync(CancellationToken ct);


    Task DeleteAsync(int id, CancellationToken ct);

    Task<bool> UpdateAsync(int id, UpdateStudentInput input, CancellationToken ct);
}
