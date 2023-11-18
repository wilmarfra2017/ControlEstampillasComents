using Azure.Core;
using VentaControlEstampillas.Domain.Exceptions;
using System.Net;

namespace VentaControlEstampillas.Api.Middleware;

/*
 * es un middleware que se coloca en el pipeline de procesamiento de solicitudes de una aplicación ASP.NET Core para capturar, 
 * registrar y responder a excepciones no controladas de una manera uniforme. 
 * Si ocurre alguna excepción mientras se procesa una solicitud, este middleware asegura que la respuesta tenga un formato JSON coherente y un código de estado HTTP apropiado. 
 */


public class AppExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AppExceptionHandlerMiddleware> _logger;

    public AppExceptionHandlerMiddleware(RequestDelegate next, ILogger<AppExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            
            var result = System.Text.Json.JsonSerializer.Serialize(new
            {               
                ErrorMessage = ex.Message
            });

            context.Response.ContentType = ContentType.ApplicationJson.ToString();
            context.Response.StatusCode = 
                (ex is CoreBusinessException) ? (int)HttpStatusCode.BadRequest : (int)HttpStatusCode.InternalServerError; 
            await context.Response.WriteAsync(result);
        }
    }
}
