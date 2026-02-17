using CourseHub.Application.Abstractions.Persistence;

namespace CourseHub.Infrastructure.Entities;

public class CourseInstanceEntity : IEntity<int>
{
    public int Id { get; set; }

    //FK
    public int CourseId { get; set; }
    public CourseEntity Course { get; set; } = null!;
    // FK
    public int TeacherId { get; set; }
    public TeacherEntity Teacher { get; set; } = null!;


    public DateOnly StartDateUtc { get; set; }
    public DateOnly EndDateUtc { get; set; }
    public string Location { get; set; } = null!;
    public int Capacity { get; set; }

    public ICollection<EnrollmentEntity> Enrollments { get; set; } = [];

}
