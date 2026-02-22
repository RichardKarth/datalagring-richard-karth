namespace CourseHub.Application.Courses.Inputs;

public sealed class UpdateCourseInput
{
    public string Title { get; init; } = null!;
    public string? Description { get; init; }
    public int DurationDays { get; init; }
}