using Business.Services.Helpers;
using Business.Services.Implementations;
using Business.Services.Interfaces;
using DataAccess.Context;
using DataAccess.Repositories.Implementations;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. DATABASE CONFIGURATION
// ==========================================
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ==========================================
// 2. DEPENDENCY INJECTION (Layer Connections)
// ==========================================
// Data Access Layer (Repositories)
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IProjectTaskRepository, ProjectTaskRepository>();
builder.Services.AddScoped<IActivityLogRepository, ActivityLogRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();

// Business Layer (Services)
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IProjectTaskService, ProjectTaskService>();
builder.Services.AddScoped<IAdminDashboardService, AdminDashboardService>();

//// Helpers (Infrastructure/Utilities)
builder.Services.AddScoped<IImageHelper, ImageHelper>();


// Add services to the container.

builder.Services.AddControllers();

// CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .WithExposedHeaders("X-User-Guid");
        });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowAngular");

app.UseAuthorization();

// Without this images cannot be accessed publicly.
app.UseStaticFiles();

app.MapControllers();

app.Run();
