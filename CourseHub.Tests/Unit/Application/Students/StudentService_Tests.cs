using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CourseHub.Application.Abstractions.Persistence;
using CourseHub.Application.Abstractions.Persistence.Repositories;
using CourseHub.Application.Students;
using CourseHub.Application.Students.Inputs;
using CourseHub.Application.Students.PersistanceModels;
using CourseHub.Domain.Students.ValueObjects;
using NSubstitute;
using NUnit.Framework;

namespace CourseHub.Tests.Students;

[TestFixture]
public class StudentServiceTests
{
    private IStudentRepository _repo = null!;
    private IUnitOfWork _uow = null!;
    private StudentService _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _repo = Substitute.For<IStudentRepository>();
        _uow = Substitute.For<IUnitOfWork>();
        _sut = new StudentService(_repo, _uow);
    }

    [Test]
    public async Task CreateAsync_ShouldAddStudent_AndSaveChanges()
    {
        // Arrange
        var ct = CancellationToken.None;
        var input = new CreateStudentInput(
            "John",
            "Doe",
            "john@domain.com",
            "0701234567"
        );

        // Act
        await _sut.CreateAsync(input, ct);

        // Assert
        await _repo.Received(1).AddAsync(
            Arg.Is<Student>(s =>
                s.Id == 0 &&
                s.FirstName == input.FirstName &&
                s.LastName == input.LastName &&
                s.Email == new Email(input.Email).Value && // Email VO normalized
                s.PhoneNumber == input.PhoneNumber &&
                s.CreatedAtUtc != default &&
                s.ModifiedAtUtc != default
            ),
            ct
        );

        await _uow.Received(1).SaveChangesAsync(ct);
    }

    [Test]
    public async Task GetByIdAsync_WhenStudentExists_ReturnsOutput()
    {
        // Arrange
        var ct = CancellationToken.None;

        var student = new Student
        {
            Id = 10,
            FirstName = "A",
            LastName = "B",
            Email = "a@b.com",
            PhoneNumber = "070",
            CreatedAtUtc = DateTime.UtcNow.AddDays(-1),
            ModifiedAtUtc = DateTime.UtcNow,
            RowVersion = Array.Empty<byte>()
        };

        _repo.GetByIdAsync(10, ct).Returns(student);

        // Act
        var result = await _sut.GetByIdAsync(10, ct);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(10));
        Assert.That(result.FirstName, Is.EqualTo("A"));
        Assert.That(result.LastName, Is.EqualTo("B"));
        Assert.That(result.Email, Is.EqualTo("a@b.com"));
        Assert.That(result.PhoneNumber, Is.EqualTo("070"));
    }

    [Test]
    public async Task GetByIdAsync_WhenStudentDoesNotExist_ReturnsNull()
    {
        // Arrange
        var ct = CancellationToken.None;
        _repo.GetByIdAsync(999, ct).Returns((Student?)null);

        // Act
        var result = await _sut.GetByIdAsync(999, ct);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetAllAsync_ReturnsConvertedList()
    {
        // Arrange
        var ct = CancellationToken.None;

        var list = new List<Student>
        {
            new()
            {
                Id = 1, FirstName = "One", LastName = "Test", Email = "one@test.com", PhoneNumber = null,
                CreatedAtUtc = DateTime.UtcNow, ModifiedAtUtc = DateTime.UtcNow, RowVersion = Array.Empty<byte>()
            },
            new()
            {
                Id = 2, FirstName = "Two", LastName = "Test", Email = "two@test.com", PhoneNumber = "070",
                CreatedAtUtc = DateTime.UtcNow, ModifiedAtUtc = DateTime.UtcNow, RowVersion = Array.Empty<byte>()
            }
        };

        _repo.ListAsync(ct).Returns(list);

        // Act
        var result = await _sut.GetAllAsync(ct);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result.Select(x => x.Id), Is.EquivalentTo(new[] { 1, 2 }));
        Assert.That(result.First(x => x.Id == 1).Email, Is.EqualTo("one@test.com"));
        Assert.That(result.First(x => x.Id == 2).PhoneNumber, Is.EqualTo("070"));
    }

    [Test]
    public async Task UpdateAsync_WhenStudentDoesNotExist_ReturnsFalse_AndDoesNotSave()
    {
        // Arrange
        var ct = CancellationToken.None;
        _repo.GetByIdAsync(123, ct).Returns((Student?)null);

        var input = new UpdateStudentInput
        {
            FirstName = "New",
            LastName = "Name",
            Email = "new@example.com",
            PhoneNumber = "0709999999"
        };

        // Act
        var ok = await _sut.UpdateAsync(123, input, ct);

        // Assert
        Assert.That(ok, Is.False);
        await _repo.DidNotReceive().UpdateAsync(Arg.Any<Student>(), ct);
        await _uow.DidNotReceive().SaveChangesAsync(ct);
    }

    [Test]
    public async Task UpdateAsync_WhenStudentExists_UpdatesEntity_CallsRepoUpdate_AndSaves()
    {
        // Arrange
        var ct = CancellationToken.None;

        var student = new Student
        {
            Id = 5,
            FirstName = "Old",
            LastName = "Old",
            Email = "old@example.com",
            PhoneNumber = null,
            CreatedAtUtc = DateTime.UtcNow.AddDays(-10),
            ModifiedAtUtc = DateTime.UtcNow.AddDays(-10),
            RowVersion = new byte[] { 1, 2, 3 }
        };

        _repo.GetByIdAsync(5, ct).Returns(student);

        var input = new UpdateStudentInput
        {
            FirstName = "New",
            LastName = "Name",
            Email = "new@example.com",
            PhoneNumber = "0709999999"
        };

        // Act
        var ok = await _sut.UpdateAsync(5, input, ct);

        // Assert
        Assert.That(ok, Is.True);

        Assert.That(student.FirstName, Is.EqualTo("New"));
        Assert.That(student.LastName, Is.EqualTo("Name"));
        Assert.That(student.Email, Is.EqualTo("new@example.com"));
        Assert.That(student.PhoneNumber, Is.EqualTo("0709999999"));
        Assert.That(student.ModifiedAtUtc, Is.Not.EqualTo(DateTime.MinValue));

        await _repo.Received(1).UpdateAsync(Arg.Is<Student>(s => ReferenceEquals(s, student)), ct);

        await _uow.Received(1).SaveChangesAsync(ct);
    }

    [Test]
    public void DeleteAsync_WhenStudentDoesNotExist_ThrowsArgumentException()
    {
        // Arrange
        var ct = CancellationToken.None;
        _repo.GetByIdAsync(404, ct).Returns((Student?)null);

        // Act + Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _sut.DeleteAsync(404, ct));
        Assert.That(ex!.Message, Is.EqualTo("Student not found"));
    }

    [Test]
    public async Task DeleteAsync_WhenStudentExists_DeletesById_AndSavesChanges()
    {
        // Arrange
        var ct = CancellationToken.None;

        var student = new Student
        {
            Id = 7,
            FirstName = "X",
            LastName = "Y",
            Email = "x@y.com",
            PhoneNumber = null,
            CreatedAtUtc = DateTime.UtcNow.AddDays(-1),
            ModifiedAtUtc = DateTime.UtcNow.AddDays(-1),
            RowVersion = new byte[] { 9, 9, 9 }
        };

        _repo.GetByIdAsync(7, ct).Returns(student);

        // Act
        await _sut.DeleteAsync(7, ct);

        await _repo.Received(1).UpdateAsync(student, ct);

        await _repo.Received(1).DeleteByIdAsync(7, ct);
        await _uow.Received(1).SaveChangesAsync(ct);
    }
}
