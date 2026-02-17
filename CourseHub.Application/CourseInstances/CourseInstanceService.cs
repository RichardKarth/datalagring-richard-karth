using CourseHub.Application.Abstractions.Persistence;
using CourseHub.Application.Abstractions.Persistence.Repositories;
using CourseHub.Application.Abstractions.Services;
using CourseHub.Application.CourseInstances.Inputs;
using CourseHub.Application.CourseInstances.Outputs;
using CourseHub.Application.CourseInstances.PersistanceModels;



public sealed class CourseInstanceService(ICourseInstanceRepository repo, IUnitOfWork uow) : ICourseInstanceService
{

    private static CourseInstanceOutput ToOutputModel(CourseInstance cI)
    {
        CourseInstanceOutput courseOutput = new CourseInstanceOutput
        {
            Id = cI.Id,
            CourseId = cI.CourseId,
            TeacherId = cI.TeacherId,
            StartDateUtc = cI.StartDateUtc,
            EndDateUtc = cI.EndDateUtc,
            Location = cI.Location,
            Capacity = cI.Capacity,

        };
        return courseOutput;
    }

    public async Task CreateAsync(CreateCourseInstanceInput input, CancellationToken ct)
    {
        var model = new CourseInstance
        {
            CourseId = input.CourseId,
            TeacherId = input.TeacherId,
            StartDateUtc = input.StartDateUtc,
            EndDateUtc = input.EndDateUtc,
            Location = input.Location,
            Capacity = input.Capacity
        };

        await repo.AddAsync(model, ct);
        await uow.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<CourseInstanceOutput>> GetAllAsync(CancellationToken ct)
        => (await repo.ListAsync(ct)).Select(ToOutputModel).ToList();

    public async Task<CourseInstanceOutput?> GetByIdAsync(int id, CancellationToken ct)
    {
        var instance = await repo.GetByIdAsync(id, ct);
        return instance is null ? null : ToOutputModel(instance);
    }

    public async Task DeleteAsync(int id, CancellationToken ct)
    {
        await repo.DeleteByIdAsync(id, ct);
        await uow.SaveChangesAsync(ct);
    }
}
