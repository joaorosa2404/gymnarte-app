using GymnArteApp.Server.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Yarp.ReverseProxy;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<GymDbContext>(options =>
    options.UseNpgsql(connectionString));

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

//Reverse Proxy
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
builder.Services.AddHttpClient("Clients.gymnarteapp.backoffice", client =>
{
    client.Timeout = TimeSpan.FromSeconds(60);
});
builder.Services.AddHttpClient("Clients.gymnarteapp.client", client =>
{
    client.Timeout = TimeSpan.FromSeconds(60);
});

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

builder.Services.AddCors(options =>
{
    options.AddPolicy("GymnArteClientsPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:5174") // Portas do Vite
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

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

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("GymnArteClientsPolicy");

app.MapControllers();
app.MapGroup("/auth").MapIdentityApi<IdentityUser>();
app.MapFallbackToFile("/index.html");
app.MapReverseProxy();

app.Run();
