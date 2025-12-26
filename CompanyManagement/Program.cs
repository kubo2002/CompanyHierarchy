using CompanyManagement.Api.Middleware;
using CompanyManagement.Application.Abstractions.Repositories;
using CompanyManagement.Application.UseCases;
using CompanyManagement.Infrastructure.Persistence;
using CompanyManagement.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using CompanyManagement.Api.Responses;

var builder = WebApplication.CreateBuilder(args);

// Nacitanie connection stringu z konfiguracie (appsettings.json)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string not found");


// vsuvka kvoli prebijaniu standardneho ValidationProblemDetails oproti mojmu zadefinovanemu formatu error response
// Vypína defaultný ProblemDetails pre modeling binding errors


// MVC kontrolery
builder.Services.AddControllers();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(x => x.Value!.Errors.Count > 0)
            .ToDictionary(
                x => x.Key,
                x => x.Value!.Errors
                    .Select(e => e.ErrorMessage)
                    .ToArray()
            );

        return new BadRequestObjectResult(new
        {
            success = false,
            message = "Validation failed",
            errors
        });
    };
});
// pre Scalar 
builder.Services.AddOpenApi();

// Registracia DbContextu – SQL Server ako perzistentna databaza
builder.Services.AddDbContext<ManagementDbContext>(options => options.UseSqlServer(connectionString));

// Registracia repository vrstvy (Infrastructure -> Application abstractions)
builder.Services.AddScoped<INodeRepository, EfNodeRepository>();
builder.Services.AddScoped<IEmployeeRepository, EfEmployeeRepository>();
builder.Services.AddScoped<IDepartmentEmployeeRepository, EfDepartmentEmployeeRepository>();

// Registracia Use Case-ov z Aplication vrstvy
builder.Services.AddScoped<CreateNode>();
builder.Services.AddScoped<CreateEmployee>();
builder.Services.AddScoped<AssignManagerToNode>();
builder.Services.AddScoped<UnassignManagerFromNode>();
builder.Services.AddScoped<AssignEmployeeToNode>();
builder.Services.AddScoped<RemoveEmployeeFromNode>();
builder.Services.AddScoped<GetNodeTree>();
builder.Services.AddScoped<GetNodesByType>();
builder.Services.AddScoped<DeleteNode>();
builder.Services.AddScoped<DeleteEmployee>();
builder.Services.AddScoped<UnassignEmployeeFromDepartment>();
builder.Services.AddScoped<GetEmployeesByDepartment>();
builder.Services.AddScoped<UpdateNode>();
builder.Services.AddScoped<UpdateEmployee>();

var app = builder.Build();

// Zobrazenie connection stringu a DB_NAME() pre diagnostiku (ci som pripojeny na databazu na ktoru chcem)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ManagementDbContext>();
    var conn = db.Database.GetDbConnection();

    Console.WriteLine("=== EF CONNECTION STRING ===");
    Console.WriteLine(conn.ConnectionString);

    conn.Open();
    using var cmd = conn.CreateCommand();
    cmd.CommandText = "SELECT DB_NAME()";
    Console.WriteLine("=== DB_NAME() ===");
    Console.WriteLine(cmd.ExecuteScalar());
}

// Pomocny diagnosticky check – uzitocne pri lokalnom behu alebo Docker-i
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ManagementDbContext>();

    if (db.Database.CanConnect())
    {
        Console.WriteLine("Database connection OK");

    }
    else
    {
        Console.WriteLine("Database connection FAILED");
    }
}

// HTTP pipelina ...
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// Globalny handler vynimiek
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
