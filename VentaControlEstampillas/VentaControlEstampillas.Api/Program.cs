using VentaControlEstampillas.Api.ApiHandlers;
using VentaControlEstampillas.Api.Filters;
using VentaControlEstampillas.Api.Middleware;
using FluentValidation;
using VentaControlEstampillas.Infrastructure.DataSource;
using VentaControlEstampillas.Infrastructure.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using Serilog.Debugging;
using Serilog.Sinks.Elasticsearch;
using Prometheus;


/*
 * La clase Program.cs es esencialmente la columna vertebral de la aplicación, 
 * definiendo cómo se construye, configura y ejecuta. Configura servicios, middlewares, enrutamiento, registro y otros aspectos cruciales.
 */


var builder = WebApplication.CreateBuilder(args);//Se crea un WebApplicationBuilder que ayuda a configurar y construir la aplicación.
var config = builder.Configuration;


//Se añaden al contenedor de DI los validadores definidos en el mismo ensamblado que contiene la clase Program.
builder.Services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);

//Se añade y configura un contexto de base de datos usando Entity Framework Core con SQL Server
builder.Services.AddDbContext<DataContext>(opts =>
{
    opts.UseSqlServer(config.GetConnectionString("db"));
});


//Configura las verificaciones de salud y añade una verificación específica para el contexto de la base de datos.
//Además, los resultados de las verificaciones de salud se reenvían a Prometheus, un sistema de monitorización.
builder.Services.AddHealthChecks()
    .AddDbContextCheck<DataContext>()
    .ForwardToPrometheus();

builder.Services.AddDomainServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Configura MediatR
builder.Services.AddMediatR(Assembly.Load("VentaControlEstampillas.Application"), typeof(Program).Assembly);


//Utiliza Serilog como sistema de registro. Configura el registro para escribir en la consola y en Elasticsearch, una herramienta de búsqueda y análisis.
builder.Host.UseSerilog((_, loggerConfiguration) =>
    loggerConfiguration
        .WriteTo.Console()
        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(config.GetValue<string>("esUrl")!))
        {
            TypeName = null,
            ModifyConnectionSettings = cx => cx.ServerCertificateValidationCallback((_, _, _, _) => true),
            AutoRegisterTemplate = true,
            IndexFormat = "dotnet-ms-{0:yyyy-MM-dd}",
        }));


// Habilita el registro interno de Serilog para capturar problemas que puedan surgir dentro de la propia biblioteca de Serilog
SelfLog.Enable(Console.Error);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    //Si la aplicación está en modo de desarrollo, se habilita Swagger y su interfaz de usuario.
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Redirige las solicitudes HTTP a HTTPS
app.UseHttpsRedirection();

//Habilita la recopilación de métricas HTTP para Prometheus
app.UseHttpMetrics();

//Utiliza el middleware personalizado para manejar excepciones
app.UseMiddleware<AppExceptionHandlerMiddleware>();


// Define un punto final para las verificaciones de salud en la ruta "/healthz".
app.MapHealthChecks("/healthz", new HealthCheckOptions
{
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    }
});

//Configura el enrutamiento y define un punto final para las métricas de Prometheus.
app.UseRouting().UseEndpoints(endpoint => {
    endpoint.MapMetrics();
});

//Configura el punto final para la ruta "/api/voter" y aplica el filtro de validación.
app.MapGroup("/api/voter").MapVoter().AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory);

//Inicia y ejecuta la aplicación
app.Run();

