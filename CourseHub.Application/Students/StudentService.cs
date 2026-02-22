
using CourseHub.Application.Abstractions.Persistence;
using CourseHub.Application.Abstractions.Persistence.Repositories;
using CourseHub.Application.Abstractions.Services;
using CourseHub.Application.Students.Inputs;
using CourseHub.Application.Students.Outputs;
using CourseHub.Application.Students.PersistanceModels;
using CourseHub.Domain.Students.ValueObjects;

namespace CourseHub.Application.Students;

public sealed class StudentService(IStudentRepository studentRepository, IUnitOfWork uow) : IStudentService
{
    private static StudentProfileOutput ToOutputModel(Student s)
    {
        StudentProfileOutput student = new(s.Id, s.FirstName, s.LastName, s.Email, s.PhoneNumber);
        return student;
    }



    public async Task CreateAsync(CreateStudentInput input, CancellationToken ct)
    {
        var email = new Email(input.Email);
        var createdAt = DateTime.UtcNow;

        Student student = new Student
        {
            Id = 0,
            Email = email.Value,
            FirstName = input.FirstName,
            LastName = input.LastName,
            PhoneNumber = input.PhoneNumber,
            CreatedAtUtc = createdAt,
            ModifiedAtUtc = createdAt,
            RowVersion = Array.Empty<byte>()


        };
        await studentRepository.AddAsync(student, ct);

       await uow.SaveChangesAsync(ct);

        
    }

    public async Task DeleteAsync(int id, CancellationToken ct)
    {
        var student = await studentRepository.GetByIdAsync(id, ct);
        if (student == null)
        {
            throw new ArgumentException("Student not found");
        }
        await studentRepository.UpdateAsync(student, ct);

        await studentRepository.DeleteByIdAsync(student.Id, ct);

        await uow.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<StudentProfileOutput>> GetAllAsync(CancellationToken ct)
    {
        var list = await studentRepository.ListAsync(ct);
        //Ta listan av Student skicka till out ToOutput som converterar till StudentProfileOutput
        return list.Select(x => ToOutputModel(x)).ToList();
    }

    public async Task<StudentProfileOutput?> GetByIdAsync(int id, CancellationToken ct)
    {
        var student = await studentRepository.GetByIdAsync(id, ct);
        return student is null ? null : ToOutputModel(student);
    }
    public async Task<bool> UpdateAsync(int id, UpdateStudentInput input, CancellationToken ct)
    {
        var entity = await studentRepository.GetByIdAsync(id, ct);
        if (entity is null) return false;

        entity.FirstName = input.FirstName;
        entity.LastName = input.LastName;
        entity.Email = input.Email;
        entity.PhoneNumber = input.PhoneNumber;
        entity.ModifiedAtUtc = DateTime.UtcNow;

        await uow.SaveChangesAsync(ct);
        return true;
    }
}
