using CourseHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();

builder.Services.AddDbContext<CHDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DatabaseCH"),
    sql => sql.MigrationsAssembly("CourseHub.Infrastructure")
 ));

var app = builder.Build();


app.MapOpenApi();
app.UseHttpsRedirection();

app.Run();


