using GithubClone.Application.Interfaces.Repository;
using GithubClone.Application.Repository;
using GithubClone.Infrastructure.Database;
using GithubClone.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

// ---------------- Configuration ----------------

// Register DapperContext (reads connection string from appsettings.json)
builder.Services.AddSingleton<DapperContext>();

// ---------------- Repositories ----------------

builder.Services.AddScoped<IUserRepository, UserRepository>();

// ---------------- Controllers ----------------

builder.Services.AddControllers();

// ---------------- Swagger ----------------

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ---------------- Middleware ----------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();