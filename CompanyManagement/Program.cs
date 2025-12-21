using CompanyManagement.Application.Abstractions.Repositories;
using CompanyManagement.Infrastructure.Persistence;
using CompanyManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

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

var app = builder.Build();

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
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
