
namespace CourseHub.Domain.Entities;

public class EnrollmentEntity
{
    public int Id { get; private set; }

    public int StudentId { get; private set; }

    public int CourseInstanceId { get; private set; }

    public DateTime RegisteredAt { get; private set; }
    
}
