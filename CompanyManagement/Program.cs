using CompanyManagement.Api.Middleware;
using CompanyManagement.Application.Abstractions.Repositories;
using CompanyManagement.Application.UseCases;
using CompanyManagement.Infrastructure.Persistence;
using CompanyManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Nacitanie connection stringu z konfiguracie (appsettings.json)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string not found");


// MVC kontrolery
builder.Services.AddControllers();

// pre Scalar 
builder.Services.AddOpenApi();

// Registracia DbContextu – SQL Server ako perzistentna databaza
builder.Services.AddDbContext<ManagementDbContext>(options => options.UseSqlServer(connectionString));

// Registracia repository vrstvy (Infrastructure -> Application abstractions)
builder.Services.AddScoped<INodeRepository, EfNodeRepository>();
builder.Services.AddScoped<IEmployeeRepository, EfEmployeeRepository>();

// Registracia Use Case-ov z Aplication vrstvy
builder.Services.AddScoped<CreateNode>();
builder.Services.AddScoped<CreateEmployee>();
builder.Services.AddScoped<AssignManagerToNode>();
builder.Services.AddScoped<UnassignManagerFromNode>();
builder.Services.AddScoped<AssignEmployeeToNode>();
builder.Services.AddScoped<RemoveEmployeeFromNode>();


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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
