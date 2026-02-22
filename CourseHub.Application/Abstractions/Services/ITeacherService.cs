

using CourseHub.Application.Teachers.Inputs;
using CourseHub.Application.Teachers.Outputs;
using System.ComponentModel;

namespace CourseHub.Application.Abstractions.Services;

public interface ITeacherService
{
    Task CreateAsync(TeacherInput input, CancellationToken ct);
    Task<TeacherOutput?> GetByIdAsync(int id, CancellationToken ct);
    Task<IReadOnlyList<TeacherOutput>> GetAllAsync(CancellationToken ct);
    Task DeleteByIdAsync(int id, CancellationToken ct);
    Task<bool> UpdateAsync(int id, UpdateTeacherInput input, CancellationToken ct);
}
