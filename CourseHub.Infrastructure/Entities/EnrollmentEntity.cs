namespace CourseHub.Infrastructure.Entities;

public class EnrollmentEntity
{
    //JOIN TABLE
    public int Id { get; set; }

    //FK
    public int StudentId { get; set; }
    public StudentEntity Student { get; set; } = null!;

    //FK
    public int CourseInstanceId { get; set; }
    public CourseInstanceEntity CourseInstance { get; set; } = null!;
}
