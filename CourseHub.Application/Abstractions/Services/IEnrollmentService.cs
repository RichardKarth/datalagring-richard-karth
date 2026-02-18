

namespace CourseHub.Application.Abstractions.Services;

public interface IEnrollmentService
{
    Task EnrollAsync(int studentId, int courseInstanceId, CancellationToken ct);
    Task UnenrollAsync(int studentId, int courseInstanceId, CancellationToken ct);

    Task<IReadOnlyList<int>> ListStudentIdsInInstanceAsync(int courseInstanceId, CancellationToken ct);
    Task<IReadOnlyList<int>> ListCourseInstanceIdsForStudentAsync(int studentId, CancellationToken ct);
}
