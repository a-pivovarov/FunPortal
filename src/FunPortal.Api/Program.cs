using FunPortal.Api.Extensions;
using FunPortal.Api.Middleware;
using FunPortal.Application.Interfaces.Persistence;
using FunPortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Add OpenAPI/Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

// Add DbContext with SQLite
builder.Services.AddDbContext<FunPortalDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") 
        ?? "Data Source=funportal.db"));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add MediatR and FluentValidation
builder.Services.AddMediatR();

// Add Repositories
builder.Services.AddRepositories();

// Add Services
builder.Services.AddServices();

// Add Exception Handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FunBooksAndVideos API v1");
    });
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
