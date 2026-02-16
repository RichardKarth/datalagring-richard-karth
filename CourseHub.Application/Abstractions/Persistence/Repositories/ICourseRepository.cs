using CourseHub.Application.Courses.PersistanceModels;


namespace CourseHub.Application.Abstractions.Persistence.Repositories
{
    public interface ICourseRepository : IRepositoryBase<Course, int>
    {
    }
}
