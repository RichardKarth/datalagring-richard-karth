namespace CourseHub.Application.CourseInstances.Inputs;

public sealed class UpdateCourseInstanceInput
{
    public int CourseId { get; init; }
    public int TeacherId { get; init; }

    // frontend skickar "YYYY-MM-DD"
    public DateOnly StartDateUtc { get; init; }
    public DateOnly EndDateUtc { get; init; }

    public string Location { get; init; } = null!;
    public int Capacity { get; init; }
}