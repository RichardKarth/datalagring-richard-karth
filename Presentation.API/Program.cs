using CourseHub.Application.Abstractions.Persistence;
using CourseHub.Application.Abstractions.Persistence.Repositories;
using CourseHub.Application.Abstractions.Services;
using CourseHub.Application.CourseInstances.Inputs;
using CourseHub.Application.Courses;
using CourseHub.Application.Courses.Inputs;
using CourseHub.Application.Enrollments;
using CourseHub.Application.Enrollments.Inputs;
using CourseHub.Application.Students;
using CourseHub.Application.Students.Inputs;
using CourseHub.Application.Teachers;
using CourseHub.Application.Teachers.Inputs;
using CourseHub.Infrastructure.Data;
using CourseHub.Infrastructure.Repositories;
using CourseHub.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

//DBcontext
builder.Services.AddDbContext<CHDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DatabaseCH"),
    sql => sql.MigrationsAssembly("CourseHub.Infrastructure")
 ));

//Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("react", p =>
        p.WithOrigins("http://localhost:5173")
         .AllowAnyHeader()
         .AllowAnyMethod());
});

//Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Repositories
builder.Services.AddScoped<IStudentRepository, StudentEntityRepository>();
builder.Services.AddScoped<ICourseRepository, CourseEntityRepository>();
builder.Services.AddScoped<ITeacherRepository, TeacherEntityRepository>();
builder.Services.AddScoped<ICourseInstanceRepository, CourseInstanceEntityRepository>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();

// Services
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddScoped<ICourseInstanceService, CourseInstanceService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();

var app = builder.Build();

app.UseCors("react");
app.MapOpenApi();
app.UseHttpsRedirection();


//STUDENTS ---------------------------------------------

var students = app.MapGroup("/students");

students.MapGet("/", async (IStudentService service, CancellationToken ct)
    => Results.Ok(await service.GetAllAsync(ct)));

students.MapGet("/{id:int}", async (int id, IStudentService service, CancellationToken ct) =>
{
    var student = await service.GetByIdAsync(id, ct);
    return student is null ? Results.NotFound() : Results.Ok(student);
});

students.MapPost("/", async (CreateStudentInput input, IStudentService service, CancellationToken ct) =>
{
    await service.CreateAsync(input, ct);
    return Results.Ok();
});

students.MapDelete("/{id:int}", async (int id, IStudentService service, CancellationToken ct) =>
{
    await service.DeleteAsync(id, ct);
    return Results.NoContent();
});


//COURSES ---------------------------------------------

var courses = app.MapGroup("/courses");

courses.MapGet("/", async (ICourseService service, CancellationToken ct)
    => Results.Ok(await service.GetAllAsync(ct)));

courses.MapGet("/{id:int}", async (int id, ICourseService service, CancellationToken ct) =>
{
    var course = await service.GetByIdAsync(id, ct);
    return course is null ? Results.NotFound() : Results.Ok(course);
});

courses.MapPost("/", async (CreateCourseInput input, ICourseService service, CancellationToken ct) =>
{
    await service.CreateAsync(input, ct);
    return Results.Ok();
});

courses.MapDelete("/{id:int}", async (int id, ICourseService service, CancellationToken ct) =>
{
    await service.DeleteAsync(id, ct);
    return Results.NoContent();
});


// TEACHERS ------------------------------------------

var teachers = app.MapGroup("/teachers");

teachers.MapGet("/", async (ITeacherService service, CancellationToken ct)
    => Results.Ok(await service.GetAllAsync(ct)));

teachers.MapGet("/{id:int}", async (int id, ITeacherService service, CancellationToken ct) =>
{
    var teacher = await service.GetByIdAsync(id, ct);
    return teacher is null ? Results.NotFound() : Results.Ok(teacher);
});

teachers.MapPost("/", async (TeacherInput input, ITeacherService service, CancellationToken ct) =>
{
    await service.CreateAsync(input, ct);
    return Results.Ok();
});

teachers.MapDelete("/{id:int}", async (int id, ITeacherService service, CancellationToken ct) =>
{
    await service.DeleteByIdAsync(id, ct);
    return Results.NoContent();
});

//COURSE INSTANCES-------------------------------------
var instances = app.MapGroup("/course-instances");

instances.MapGet("/", async (ICourseInstanceService service, CancellationToken ct)
    => Results.Ok(await service.GetAllAsync(ct)));

instances.MapGet("/{id:int}", async (int id, ICourseInstanceService service, CancellationToken ct) =>
{
    var instance = await service.GetByIdAsync(id, ct);
    return instance is null ? Results.NotFound() : Results.Ok(instance);
});

instances.MapPost("/", async (CreateCourseInstanceInput input, ICourseInstanceService service, CancellationToken ct) =>
{
    await service.CreateAsync(input, ct);
    return Results.Ok();
});

instances.MapDelete("/{id:int}", async (int id, ICourseInstanceService service, CancellationToken ct) =>
{
    await service.DeleteAsync(id, ct);
    return Results.NoContent();
});

//ENROLLMENTS-------------------------------------------

var enrollments = app.MapGroup("/enrollments");

enrollments.MapPost("/", async (EnrollmentStudentInput input, IEnrollmentService service, CancellationToken ct) =>
{
    await service.EnrollAsync(input.StudentId, input.CourseInstanceId, ct);
    return Results.NoContent();
});

enrollments.MapDelete("/students/{studentId:int}/instances/{courseInstanceId:int}",
    async (int studentId, int courseInstanceId, IEnrollmentService service, CancellationToken ct) =>
    {
        await service.UnenrollAsync(studentId, courseInstanceId, ct);
        return Results.NoContent();
    });

enrollments.MapGet("/instances/{courseInstanceId:int}/students",
    async (int courseInstanceId, IEnrollmentService service, CancellationToken ct)
        => Results.Ok(await service.ListStudentIdsInInstanceAsync(courseInstanceId, ct)));

enrollments.MapGet("/students/{studentId:int}/instances",
    async (int studentId, IEnrollmentService service, CancellationToken ct)
        => Results.Ok(await service.ListCourseInstanceIdsForStudentAsync(studentId, ct)));

app.Run();


