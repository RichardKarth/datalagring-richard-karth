using CourseHub.Application.Abstractions.Persistence;
using CourseHub.Application.Abstractions.Persistence.Repositories;
using CourseHub.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseHub.Application.Enrollments;

public sealed class EnrollmentService(IEnrollmentRepository enrollments, IStudentRepository students, ICourseInstanceRepository courseInstances, IUnitOfWork uow) : IEnrollmentService
{
    public async Task EnrollAsync(int studentId, int courseInstanceId, CancellationToken ct)
    {
        var student = await students.GetByIdAsync(studentId, ct);
        if (student is null)
            throw new ArgumentException("Student not found");

        var instance = await courseInstances.GetByIdAsync(courseInstanceId, ct);
        if (instance is null)
            throw new ArgumentException("Course instance not found");

        if (await enrollments.ExistsAsync(studentId, courseInstanceId, ct))
            throw new ArgumentException("Student is already enrolled in this course instance");

        var currentCount = await enrollments.CountByCourseInstanceAsync(courseInstanceId, ct);
        if (currentCount >= instance.Capacity)
            throw new ArgumentException("Course instance is full");

        await enrollments.AddAsync(studentId, courseInstanceId, ct);
        await uow.SaveChangesAsync(ct);
    }

    public async Task UnenrollAsync(int studentId, int courseInstanceId, CancellationToken ct)
    {
        if (!await enrollments.ExistsAsync(studentId, courseInstanceId, ct))
            throw new ArgumentException("Enrollment not found");

        await enrollments.DeleteAsync(studentId, courseInstanceId, ct);
        await uow.SaveChangesAsync(ct);
    }

    public Task<IReadOnlyList<int>> ListStudentIdsInInstanceAsync(int courseInstanceId, CancellationToken ct)
        => enrollments.ListStudentIdsByCourseInstanceAsync(courseInstanceId, ct);

    public Task<IReadOnlyList<int>> ListCourseInstanceIdsForStudentAsync(int studentId, CancellationToken ct)
        => enrollments.ListCourseInstanceIdsByStudentAsync(studentId, ct);
}