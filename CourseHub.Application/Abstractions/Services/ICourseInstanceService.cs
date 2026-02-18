using CourseHub.Application.CourseInstances.Inputs;
using CourseHub.Application.CourseInstances.Outputs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseHub.Application.Abstractions.Services
{
    public interface ICourseInstanceService
    {
        Task CreateAsync(CreateCourseInstanceInput input, CancellationToken ct);
        Task<IReadOnlyList<CourseInstanceOutput>> GetAllAsync(CancellationToken ct);
        Task<CourseInstanceOutput?> GetByIdAsync(int id, CancellationToken ct);
        Task DeleteAsync(int id, CancellationToken ct);
    }
}
