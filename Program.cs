using EncryptorService.Services;
using Futbol_Insight_Jobs.Data;
using Futbol_Insight_Jobs.Middleware;
using Futbol_Insight_Jobs.Services.Access;
using Futbol_Insight_Jobs.Services.ApiService;
using Futbol_Insight_Jobs.Services.Country;
using Futbol_Insight_Jobs.Tools;
using LogServiceYiDev.Data;
using LogServiceYiDev.Interfaces;
using LogServiceYiDev.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();



builder.Services.AddSingleton<Utilities>();

builder.Services.AddDbContext<DataContext>();
builder.Services.AddDbContext<AdmonContext>();
builder.Services.AddDbContext<LogDataContext>();


// Configuración de servicios
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<ApiContext>();
builder.Services.AddScoped<ICountry, Country>();
builder.Services.AddScoped<IAccess, Access>();
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddScoped<IEncryptionService, EncryptionService>();


builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
 }).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };

    config.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            // Reemplaza 'Authorization' con 'apitoken'
            var token = context.Request.Headers["ApiTokenJob"].FirstOrDefault();
            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token;
            }
            else
            {
                context.NoResult();
            }
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            // Evita la respuesta automática
            context.HandleResponse();

            // Personaliza la respuesta con un JSON
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            var result = JsonSerializer.Serialize(new
            {
                code = 401,
                message = "You are not authorized to access this resource",
                details = "Unauthorized",
                data = false
            });
            return context.Response.WriteAsync(result);
        }
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("NewPolicy", app =>
    {
        app.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseMiddleware<LoggingMiddleware>();

app.UseMiddleware<RegistrationTokenMiddleware>();

app.UseCors("NewPolicy");

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
