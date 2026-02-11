
namespace CourseHub.Domain.Entities;

public class StudentEntity
{
    public int Id { get; private set; }
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string? PhoneNumber { get; private set; }
    public byte[] Concurrency { get; set; } = null!;

    public DateTime CreatedAtUtc { get; set; }
    public DateTime ModifiedAtUtc { get; set; }

    public ICollection<EnrollmentEntity> Enrollments { get; set; } = [];
}
