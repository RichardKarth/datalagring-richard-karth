using CourseHub.Application.Abstractions.Persistence;
using CourseHub.Application.Abstractions.Persistence.Repositories;
using CourseHub.Application.Abstractions.Services;
using CourseHub.Application.Courses;
using CourseHub.Application.Enrollments;
using CourseHub.Application.Students;
using CourseHub.Application.Teachers;
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


app.MapOpenApi();
app.UseHttpsRedirection();

app.Run();


