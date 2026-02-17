using CourseHub.Application.Abstractions.Persistence;

namespace CourseHub.Infrastructure.Entities;

public class TeacherEntity : IEntity<int>
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;

    public ICollection<CourseInstanceEntity> CourseInstances { get; set; } = [];
}
