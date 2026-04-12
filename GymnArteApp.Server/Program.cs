using GymnArteApp.Server.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Yarp.ReverseProxy;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//MICROSOFT IDENTITY 
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddIdentityCookies();
builder.Services.AddIdentityCore<IdentityUser>()
    .AddEntityFrameworkStores<GymDbContext>()
    .AddApiEndpoints();

// Repositories
builder.Services.AddScoped<GymnArteApp.Server.Repo.Interface.IUser, GymnArteApp.Server.Repo.User>();
builder.Services.AddScoped<GymnArteApp.Server.Repo.Interface.IUserScope, GymnArteApp.Server.Repo.UserScope>();
builder.Services.AddScoped<GymnArteApp.Server.Repo.Interface.IExercise, GymnArteApp.Server.Repo.Exercise>();
builder.Services.AddScoped<GymnArteApp.Server.Repo.Interface.IExerciseType, GymnArteApp.Server.Repo.ExerciseType>();
builder.Services.AddScoped<GymnArteApp.Server.Repo.Interface.ITrainingPlan, GymnArteApp.Server.Repo.TrainingPlan>();
builder.Services.AddScoped<GymnArteApp.Server.Repo.Interface.IExerciseTrainingPlan, GymnArteApp.Server.Repo.ExerciseTrainingPlan>();
builder.Services.AddScoped<GymnArteApp.Server.Repo.Interface.INotifications, GymnArteApp.Server.Repo.Notifications>();
builder.Services.AddScoped<GymnArteApp.Server.Repo.Interface.IBiometricData, GymnArteApp.Server.Repo.BiometricData>();

// Business & Services
builder.Services.AddScoped<GymnArteApp.Server.Business.Interface.IUser, GymnArteApp.Server.Business.User>();
builder.Services.AddScoped<GymnArteApp.Server.Business.Interface.IUserScope, GymnArteApp.Server.Business.UserScope>();
builder.Services.AddScoped<GymnArteApp.Server.Business.Interface.IExercise, GymnArteApp.Server.Business.Exercise>();
builder.Services.AddScoped<GymnArteApp.Server.Business.Interface.IExerciseType, GymnArteApp.Server.Business.ExerciseType>();
builder.Services.AddScoped<GymnArteApp.Server.Business.Interface.ITrainingPlan, GymnArteApp.Server.Business.TrainingPlan>();
builder.Services.AddScoped<GymnArteApp.Server.Business.Interface.IExerciseTrainingPlan, GymnArteApp.Server.Business.ExerciseTrainingPlan>();
builder.Services.AddScoped<GymnArteApp.Server.Business.Interface.INotifications, GymnArteApp.Server.Business.Notifications>();
builder.Services.AddScoped<GymnArteApp.Server.Business.Interface.IBiometricData, GymnArteApp.Server.Business.BiometricData>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
