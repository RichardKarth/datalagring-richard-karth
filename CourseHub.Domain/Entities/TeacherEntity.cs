
namespace CourseHub.Domain.Entities;

public class TeacherEntity
{
    public int Id { get; private set; }

    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string Email { get; private set; } = null!;
}
