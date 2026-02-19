

using CourseHub.Application.Abstractions.Persistence.Repositories;
using CourseHub.Application.Courses.Inputs;
using CourseHub.Application.Courses.Outputs;
using CourseHub.Application.Students.Inputs;
using CourseHub.Application.Students.Outputs;

namespace CourseHub.Application.Abstractions.Services
{
    public interface ICourseService
    {
        Task CreateAsync(CreateCourseInput input, CancellationToken ct);

        Task<CourseOutput?> GetByIdAsync(int id, CancellationToken ct);
        Task<IReadOnlyList<CourseOutput>> GetAllAsync(CancellationToken ct);


        Task DeleteAsync(int id, CancellationToken ct);
    }
}
