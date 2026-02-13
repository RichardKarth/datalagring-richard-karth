namespace CourseHub.Infrastructure.Entities
{
    public class CourseEntity
    {
        public int Id { get; private set; }
        public string Title { get; private set; } = null!;
        public string? Description { get; private set; }
        public int DurationDays { get; private set; }

        public ICollection<CourseInstanceEntity> CourseInstances { get; set; } = [];

    }
}
