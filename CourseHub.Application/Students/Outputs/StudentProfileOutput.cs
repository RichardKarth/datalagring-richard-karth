

namespace CourseHub.Application.Students.Outputs;

public sealed record StudentProfileOutput(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber
    

);
