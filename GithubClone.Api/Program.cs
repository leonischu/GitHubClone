using GithubClone.Application.Interfaces.Repository;
using GithubClone.Application.Interfaces.Services;
using GithubClone.Application.Mapping;
using GithubClone.Application.Repository;
using GithubClone.Application.Services;
using GithubClone.Infrastructure.Database;
using GithubClone.Infrastructure.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
//using GithubClone.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

// ---------------- Configuration ----------------

// Register DapperContext (reads connection string from appsettings.json)
builder.Services.AddSingleton<DapperContext>();

// ---------------- Repositories ----------------

builder.Services.AddScoped<IUserRepository, UserRepository>();


builder.Services.AddScoped<IAuthService, AuthServices>();


//For RepositoriesRepositories 

builder.Services.AddScoped<IRepositoryRepository, RepositoryRepository>();

builder.Services.AddScoped<IRepositoryService, RepositoryServices>();



//For Commits 
builder.Services.AddScoped<ICommitRepository, CommitRepository>();
builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<IRepositoryFileRepository, RepositoryFileRepository>();
builder.Services.AddScoped<ICommitService, CommitService>();


//For Branches
builder.Services.AddScoped<IBranchRepository, BranchRepository>();
builder.Services.AddScoped<IBranchService, BranchService>();

//For Pull Request 

builder.Services.AddScoped<IPullRequestRepository, PullRequestRepository>();
builder.Services.AddScoped<IPullRequestService, PullRequestService>();

//For Issue 
builder.Services.AddScoped<IIssueService, IssueService>();
builder.Services.AddScoped<IIssueRepository, IssueRepository>();


//For Social Features
builder.Services.AddScoped<ISocialRepository, SocialRepository>();
builder.Services.AddScoped<ISocialService, SocialService>();





try
{
    builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
}
catch (ReflectionTypeLoadException ex)
{
    foreach (var loaderException in ex.LoaderExceptions)
    {
        Console.WriteLine(loaderException?.Message);
    }
    throw;
}



// ---------------- Controllers ----------------

builder.Services.AddControllers();

// ---------------- Swagger ----------------

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});






builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            )
        };
    });

builder.Services.AddAuthorization();














var app = builder.Build();




//Global Error Handler

app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        var response = new
        {
            message = ex.Message,
            success = false
        };

        await context.Response.WriteAsJsonAsync(response);

    }
});






    // ---------------- Middleware ----------------

    if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();