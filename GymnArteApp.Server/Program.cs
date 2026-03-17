var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Repositories
builder.Services.AddSingleton<GymnArteApp.Server.Repo.UserRepository>();
builder.Services.AddSingleton<GymnArteApp.Server.Repo.ExerciseRepository>();
builder.Services.AddSingleton<GymnArteApp.Server.Repo.TrainingPlanRepository>();

// Register interfaces to implementations
builder.Services.AddSingleton<GymnArteApp.Server.Repo.Interface.IUserRepository>(sp => sp.GetRequiredService<GymnArteApp.Server.Repo.UserRepository>());
builder.Services.AddSingleton<GymnArteApp.Server.Repo.Interface.IExerciseRepository>(sp => sp.GetRequiredService<GymnArteApp.Server.Repo.ExerciseRepository>());
builder.Services.AddSingleton<GymnArteApp.Server.Repo.Interface.ITrainingPlanRepository>(sp => sp.GetRequiredService<GymnArteApp.Server.Repo.TrainingPlanRepository>());

// Services (business layer)
builder.Services.AddScoped<GymnArteApp.Server.Services.IUserService, GymnArteApp.Server.Services.UserService>();
builder.Services.AddScoped<GymnArteApp.Server.Services.IExerciseService, GymnArteApp.Server.Services.ExerciseService>();
builder.Services.AddScoped<GymnArteApp.Server.Services.ITrainingPlanService, GymnArteApp.Server.Services.TrainingPlanService>();

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
