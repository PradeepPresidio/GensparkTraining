using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TeamColabApp.Contexts;
using TeamColabApp.Interfaces;
using TeamColabApp.Models;
using TeamColabApp.Repositories;
using TeamColabApp.Services;
using TeamColabApp.Middleware;
var builder = WebApplication.CreateBuilder(args);

// Add Controllers with JSON options
builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        opts.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddTransient<IRepository<long, UserToken>, UserTokenRepository >();
builder.Services.AddTransient<IRepository<long, Project>, ProjectRepository>();
builder.Services.AddTransient<IRepository<long, ProjectTask>, ProjectTaskRepository>();
builder.Services.AddTransient<IRepository<long, Comment>, CommentRepository>();
builder.Services.AddTransient<IRepository<long, AppFile>, AppFileRepository>();
builder.Services.AddTransient<IRepository<long, Notification>, NotificationRepository>();
builder.Services.AddSignalR();



// Add DbContext
builder.Services.AddDbContext<TeamColabAppContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add Repositories
builder.Services.AddTransient<IRepository<long, User>, UserRepository>();

// Add Services
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IEncryptionService, EncryptionService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IProjectService, ProjectService>();
builder.Services.AddTransient<IProjectTaskService, ProjectTaskService>();
builder.Services.AddTransient<ICommentService, CommentService>();
builder.Services.AddTransient<IAppFileService, AppFileService>();
builder.Services.AddTransient<INotificationService, NotificationService>();

// Configure JWT Authentication
var jwtKey = builder.Configuration["Keys:JwtTokenKey"];
if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new Exception("JWT key not found in configuration.");
}
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key
        };
    });

// Add Swagger with JWT support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Team Collaboration API", Version = "v1" });

    // JWT auth support in Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter 'Bearer' followed by your token in the Authorization header.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Build app
var app = builder.Build();

// Middleware
    app.UseSwagger();
    app.UseSwaggerUI();

app.UseAuthentication();
app.UseMiddleware<TokenBlacklistMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.MapHub<NotificationHub>("/notificationHub");
app.Run();
