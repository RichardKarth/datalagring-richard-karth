using CourseHub.Application.Abstractions.Persistence;
using CourseHub.Application.Abstractions.Persistence.Repositories;
using CourseHub.Application.Abstractions.Services;
using CourseHub.Application.Courses.Inputs;
using CourseHub.Application.Courses.Outputs;
using CourseHub.Application.Courses.PersistanceModels;


namespace CourseHub.Application.Courses;

public class CourseService(ICourseRepository courseRepository, IUnitOfWork uow) : ICourseService
{
    private static CourseOutput ToOutputModel(Course c)
    {
        CourseOutput courseOutput = new CourseOutput
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            DurationDays = c.DurationDays,

        };
        return courseOutput;
    }
    public async Task CreateAsync(CreateCourseInput input, CancellationToken ct)
    {
        var createdAt = DateTime.UtcNow;

        Course course = new Course
        {
            Id = 0,
            Title = input.Title,
            Description = input.Description,
            DurationDays = input.DurationDays,
        };
        await courseRepository.AddAsync(course, ct);
        await uow.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct)
    {
        var course = await courseRepository.GetByIdAsync(id, ct);
        if (course == null)
        {
            throw new ArgumentException("Course not found");
        }

        await courseRepository.UpdateAsync(course, ct);
        await courseRepository.DeleteByIdAsync(course.Id, ct);
        await uow.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<CourseOutput>> GetAllAsync(CancellationToken ct)
    {
        var list = await courseRepository.ListAsync(ct);
       
        return list.Select(x => ToOutputModel(x)).ToList();
    }

    public async Task<CourseOutput?> GetByIdAsync(int id, CancellationToken ct)
    {
        var course = await courseRepository.GetByIdAsync(id, ct);
        if (course == null)
        {
            throw new ArgumentException($"No course with id {id} found");
        }
        var outPutModel = ToOutputModel(course);
        return outPutModel;
    }
    public async Task<bool> UpdateAsync(int id, UpdateCourseInput input, CancellationToken ct)
    {
        var entity = await courseRepository.GetByIdAsync(id, ct);
        if (entity is null) return false;

        entity.Title = input.Title;
        entity.Description = input.Description;
        entity.DurationDays = input.DurationDays;

        await uow.SaveChangesAsync(ct);
        return true;
    }
}
