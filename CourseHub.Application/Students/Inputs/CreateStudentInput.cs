

namespace CourseHub.Application.Students.Inputs;

public sealed record CreateStudentInput(
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber
    
);
