

using CourseHub.Application.Abstractions.Persistence;

namespace CourseHub.Application.Teachers.PersistanceModels
{
    public class Teacher : IEntity<int>
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
