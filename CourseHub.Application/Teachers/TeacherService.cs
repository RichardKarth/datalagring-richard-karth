using CourseHub.Application.Abstractions.Persistence;
using CourseHub.Application.Abstractions.Persistence.Repositories;
using CourseHub.Application.Abstractions.Services;
using CourseHub.Application.Courses.Outputs;
using CourseHub.Application.Courses.PersistanceModels;
using CourseHub.Application.Teachers.Inputs;
using CourseHub.Application.Teachers.Outputs;
using CourseHub.Application.Teachers.PersistanceModels;


namespace CourseHub.Application.Teachers
{
    public class TeacherService(ITeacherRepository teacherRepository, IUnitOfWork uow) : ITeacherService
    {
        private static TeacherOutput ToOutputModel(Teacher t)
        {
            TeacherOutput courseOutput = new TeacherOutput
            {
                Id = t.Id,
                FirstName = t.FirstName,
                LastName = t.LastName,
                Email = t.Email,

            };
            return courseOutput;
        }


        public async Task CreateAsync(TeacherInput input, CancellationToken ct)
        {
            Teacher teacher = new Teacher
            {
                Id = 0,
                FirstName = input.FirstName,
                LastName = input.LastName,
                Email = input.Email
            };
            await teacherRepository.AddAsync(teacher, ct);
            await uow.SaveChangesAsync(ct);
        }

        public async Task DeleteByIdAsync(int id, CancellationToken ct)
        {
            var teacher = await teacherRepository.GetByIdAsync(id, ct);
            if (teacher == null)
            {
                throw new ArgumentException("Teacher not found");
            }

            await teacherRepository.UpdateAsync(teacher, ct);
            await teacherRepository.DeleteByIdAsync(teacher.Id, ct);
            await uow.SaveChangesAsync(ct);
        }

        public async Task<IReadOnlyList<TeacherOutput>> GetAllAsync(CancellationToken ct)
        {

            var list = await teacherRepository.ListAsync(ct);

            return list.Select(x => ToOutputModel(x)).ToList(); ;
        }

        public async Task<TeacherOutput?> GetByIdAsync(int id, CancellationToken ct)
        {
            var course = await teacherRepository.GetByIdAsync(id, ct);
            if (course == null)
            {
                throw new ArgumentException($"No teacher with id {id} found");
            }
            var outPutModel = ToOutputModel(course);
            return outPutModel;
        }
        public async Task<bool> UpdateAsync(int id, UpdateTeacherInput input, CancellationToken ct)
        {
            var entity = await teacherRepository.GetByIdAsync(id, ct);
            if (entity is null) return false;

            entity.FirstName = input.FirstName;
            entity.LastName = input.LastName;
            entity.Email = input.Email;

            await teacherRepository.UpdateAsync(entity, ct);

            await uow.SaveChangesAsync(ct);
            return true;
        }
    }
}
