using System.Diagnostics;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Sideas.Challenge.Application.Services;
using Sideas.Challenge.Domain.Repositories;
using Sideas.Challenge.Infrastructure.Data;
using Sideas.Challenge.Infrastructure.Repositories;

/// <summary>
/// Punto de entrada principal de la aplicación ASP.NET Core.
/// Configura servicios, logging, dependencias, swagger y ejecuta la aplicación.
/// </summary>
var builder = WebApplication.CreateBuilder(args);

// Inicializar y configurar Serilog para logging estructurado en consola y archivo
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Fatal() // Por defecto, ignora todo
    .MinimumLevel.Override("Sideas.Challenge", Serilog.Events.LogEventLevel.Information)
    .WriteTo.Console()
    .WriteTo.File(
        "logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    .CreateLogger();

builder.Host.UseSerilog();

// Configuración de servicios y dependencias
var services = builder.Services;

// Habilitar CORS
services.AddCors(options =>
{
    options.AddPolicy("AllowAngularClient", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Angular corre por defecto en este puerto
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

services.AddControllers().AddJsonOptions(options =>
{
    // Evitar referencias cíclicas al serializar objetos (relaciones EF)
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Configurar EF Core con SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Agregar documentación de API (Swagger)
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(options =>
{
    options.SupportNonNullableReferenceTypes();
});

// Repositorios (acceso a datos)
builder.Services.AddScoped<IAgrupacionRepository, AgrupacionRepository>();
builder.Services.AddScoped<IProfesionRepository, ProfesionRepository>();
builder.Services.AddScoped<IAgrupacionProfesionRepository, AgrupacionProfesionRepository>();
builder.Services.AddScoped<IFueroRepository, FueroRepository>();
builder.Services.AddScoped<IZonaRepository, ZonaRepository>();
builder.Services.AddScoped<IAsignacionRepository, AsignacionRepository>();

// Servicios de aplicación (coordinan la lógica de negocio)
builder.Services.AddScoped<AgrupacionService>();
builder.Services.AddScoped<FueroService>();
builder.Services.AddScoped<AsignacionService>();

// Configurar servicio HTTP para consumir APIs externas
services.AddHttpClient<HttpService>();

var app = builder.Build();

app.UseCors("AllowAngularApp"); // ⬅️ Antes de app.UseAuthorization();

app.UseAuthorization();

// Configuración para entorno de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Abrir automáticamente Swagger en el navegador
    var url = "http://localhost:5190/swagger/index.html";
    try
    {
        Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
    }
    catch
    {
        // Ignorar errores al intentar abrir el navegador
    }
}

// Middleware para HTTPS y autorización
// Usar CORS con la política definida
app.UseCors("AllowAngularClient");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Iniciar la aplicación
try
{
    app.Run();
}
finally
{
    // Cerrar correctamente Serilog
    Log.CloseAndFlush();
}
