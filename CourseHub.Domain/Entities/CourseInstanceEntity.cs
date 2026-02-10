
namespace CourseHub.Domain.Entities;

public class CourseInstanceEntity
{
    public int Id { get; private set; }

    //FK
    public int CourseId { get; private set; }

    // FK
    public int TeacherId { get; private set; }

    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }
    public string Location { get; private set; } = null!;
    public int Capacity { get; private set; }

}
