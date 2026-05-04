using GymnArteApp.Server.Data;
using GymnArteApp.Server.Services;
using GymnArteApp.Server.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ── Base de Dados ─────────────────────────────────────────────────────────────
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<GymDbContext>(options =>
    options.UseNpgsql(connectionString)
           .EnableSensitiveDataLogging(builder.Environment.IsDevelopment()));

// ── Controllers + JSON (evitar ciclos de serialização) ────────────────────────
builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.ReferenceHandler      = ReferenceHandler.IgnoreCycles;
        opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// ── Swagger / OpenAPI com suporte JWT ─────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title       = "GymnArteApp API",
        Version     = "v1",
        Description = "API de gestão de ginásio — membros, planos de treino, biometria e notificações."
    });

    // Adiciona suporte ao JWT Bearer no Swagger UI
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header. Formato: **Bearer {token}**",
        Name        = "Authorization",
        In          = ParameterLocation.Header,
        Type        = SecuritySchemeType.ApiKey,
        Scheme      = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// ── JWT Bearer Authentication ─────────────────────────────────────────────────
var jwtSection = builder.Configuration.GetSection("Jwt");
var signingKey  = Encoding.UTF8.GetBytes(jwtSection["Key"]
    ?? throw new InvalidOperationException("JWT Key não configurada em appsettings.json"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = jwtSection["Issuer"],
            ValidAudience            = jwtSection["Audience"],
            IssuerSigningKey         = new SymmetricSecurityKey(signingKey),
            ClockSkew                = TimeSpan.FromMinutes(1)
        };
    });
builder.Services.AddAuthorization();

// ── CORS ──────────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("GymnArteClientsPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:5174")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// ── Serviços de Auth ──────────────────────────────────────────────────────────
builder.Services.AddScoped<IPasswordHasher<GymnArteApp.Server.Models.User>,
    PasswordHasher<GymnArteApp.Server.Models.User>>();
builder.Services.AddScoped<ITokenService, TokenService>();

// ── Repositórios ──────────────────────────────────────────────────────────────
builder.Services.AddScoped<GymnArteApp.Server.Repo.Interface.IUser,                 GymnArteApp.Server.Repo.User>();
builder.Services.AddScoped<GymnArteApp.Server.Repo.Interface.IUserScope,            GymnArteApp.Server.Repo.UserScope>();
builder.Services.AddScoped<GymnArteApp.Server.Repo.Interface.IExercise,             GymnArteApp.Server.Repo.Exercise>();
builder.Services.AddScoped<GymnArteApp.Server.Repo.Interface.IExerciseType,         GymnArteApp.Server.Repo.ExerciseType>();
builder.Services.AddScoped<GymnArteApp.Server.Repo.Interface.ITrainingPlan,         GymnArteApp.Server.Repo.TrainingPlan>();
builder.Services.AddScoped<GymnArteApp.Server.Repo.Interface.IExerciseTrainingPlan, GymnArteApp.Server.Repo.ExerciseTrainingPlan>();
builder.Services.AddScoped<GymnArteApp.Server.Repo.Interface.INotifications,        GymnArteApp.Server.Repo.Notifications>();
builder.Services.AddScoped<GymnArteApp.Server.Repo.Interface.IBiometricData,        GymnArteApp.Server.Repo.BiometricData>();

// ── Camada de Negócio ─────────────────────────────────────────────────────────
builder.Services.AddScoped<GymnArteApp.Server.Business.Interface.IUser,                 GymnArteApp.Server.Business.User>();
builder.Services.AddScoped<GymnArteApp.Server.Business.Interface.IUserScope,            GymnArteApp.Server.Business.UserScope>();
builder.Services.AddScoped<GymnArteApp.Server.Business.Interface.IExercise,             GymnArteApp.Server.Business.Exercise>();
builder.Services.AddScoped<GymnArteApp.Server.Business.Interface.IExerciseType,         GymnArteApp.Server.Business.ExerciseType>();
builder.Services.AddScoped<GymnArteApp.Server.Business.Interface.ITrainingPlan,         GymnArteApp.Server.Business.TrainingPlan>();
builder.Services.AddScoped<GymnArteApp.Server.Business.Interface.IExerciseTrainingPlan, GymnArteApp.Server.Business.ExerciseTrainingPlan>();
builder.Services.AddScoped<GymnArteApp.Server.Business.Interface.INotifications,        GymnArteApp.Server.Business.Notifications>();
builder.Services.AddScoped<GymnArteApp.Server.Business.Interface.IBiometricData,        GymnArteApp.Server.Business.BiometricData>();

var app = builder.Build();

// ── Pipeline HTTP ─────────────────────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GymnArteApp API v1");
        c.RoutePrefix = string.Empty; // Swagger na raiz (http://localhost:{port}/)
    });
}

app.UseHttpsRedirection();

// CORS deve vir antes de Auth
app.UseCors("GymnArteClientsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
