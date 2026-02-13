

namespace CourseHub.Application.Students.PersistanceModels;

public record Student(int Id, string Email, string FirstName, string LastName, string? PhoneNumber, DateTime CreatedAtUtc, DateTime ModifiedAtUtc, byte[] RowVersion );