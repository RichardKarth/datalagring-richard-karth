
namespace CourseHub.Domain.Entities;

public class CourseInstanceEntity
{
    public int Id { get; private set; }

    //FK
    public int CourseId { get; private set; }
    public CourseEntity Course { get; private set; } = null!;
    // FK
    public int TeacherId { get; private set; }
    public TeacherEntity Teacher { get; private set; } = null!;


    public DateOnly StartDateUtc { get; private set; }
    public DateOnly EndDateUtc { get; private set; }
    public string Location { get; private set; } = null!;
    public int Capacity { get; private set; }

    public ICollection<EnrollmentEntity> Enrollments = [];

}
