namespace CourseHub.Application.Teachers.Inputs;

public sealed class UpdateTeacherInput
{
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string Email { get; init; } = null!;
}