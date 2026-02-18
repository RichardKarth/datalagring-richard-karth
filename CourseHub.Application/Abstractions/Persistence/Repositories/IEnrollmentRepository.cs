using System;
using System.Collections.Generic;
using System.Text;

namespace CourseHub.Application.Abstractions.Persistence.Repositories
{
    public interface IEnrollmentRepository
    {
        //Fick hjälp av chattgpt att strukturera Enrollment Repository och Service eftersom att den ska vara så annorlunda from de andra. Den kommer inte ärva från RepositoryBase
        // eftersom att den inte är CRUD baserad.
        Task<bool> ExistsAsync(int studentId, int courseInstanceId, CancellationToken ct = default);

        Task AddAsync(int studentId, int courseInstanceId, CancellationToken ct = default);

        Task DeleteAsync(int studentId, int courseInstanceId, CancellationToken ct = default);

        Task<int> CountByCourseInstanceAsync(int courseInstanceId, CancellationToken ct = default);

        Task<IReadOnlyList<int>> ListStudentIdsByCourseInstanceAsync(int courseInstanceId, CancellationToken ct = default);

        Task<IReadOnlyList<int>> ListCourseInstanceIdsByStudentAsync(int studentId, CancellationToken ct = default);
    }
}
