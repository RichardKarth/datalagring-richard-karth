using CourseHub.Application.Abstractions.Persistence;

namespace CourseHub.Infrastructure.Entities
{
    public class CourseEntity : IEntity<int>
    {
        public int Id { get; set; } 
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int DurationDays { get; set; }

        public ICollection<CourseInstanceEntity> CourseInstances { get; set; } = [];

    }
}
