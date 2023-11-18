using VentaControlEstampillas.Api.Filters;
using VentaControlEstampillas.Application.Voters;
using MediatR;
using VentaControlEstampillas.Domain.Dto;

namespace VentaControlEstampillas.Api.ApiHandlers;

/*
 * En resumen, esta clase configura cómo la API debe responder 
 * a ciertas solicitudes relacionadas con votantes, ya sea obteniendo 
 * detalles de un votante o registrando uno nuevo.
 */

public static class VoterApi
{
    public static RouteGroupBuilder MapVoter(this IEndpointRouteBuilder routeHandler)
    {
        
        //Agregar (o "mapear") varias rutas de API relacionadas con la entidad "Voter"         
         
        // Correcto, devuelve uno filtrado por id (ef)
        routeHandler.MapGet("/{id}", async (IMediator mediator, Guid id) =>
        {
            return Results.Ok(await mediator.Send(new VoterQuery(id)));
        })
        .Produces(StatusCodes.Status200OK, typeof(VoterDto));

        // Ahora con dapper
        routeHandler.MapGet("/dapper/{id}", async (IMediator mediator, Guid id) =>
        {
            return Results.Ok(await mediator.Send(new VoterSimpleQuery(id)));
        })
        .Produces(StatusCodes.Status200OK, typeof(VoterDto));

        // Crear recurso, validando comando como controlador de api
        routeHandler.MapPost("/", async (IMediator mediator, [Validate] VoterRegisterCommand voter) =>
        {
            var vote = await mediator.Send(voter);
            return Results.Created(new Uri($"/api/voter/{vote.Id}", UriKind.Relative), vote);
        })
        .Produces(statusCode: StatusCodes.Status201Created);

        return (RouteGroupBuilder)routeHandler;
    }
}
