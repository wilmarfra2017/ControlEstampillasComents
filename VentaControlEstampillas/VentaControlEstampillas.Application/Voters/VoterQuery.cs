using VentaControlEstampillas.Domain.Dto;
using MediatR;

namespace VentaControlEstampillas.Application.Voters;


/*
 * Un record en C# es una clase especial que no permite cambios después de su creación; en lugar de modificarla, se crea una nueva copia con los cambios.
 * por eso se dice que los records son inmutables
 * */


//Estas consultas toman un uid como parametro, cuando se manejen las consultas debe devolver un VoterDto
public record VoterQuery(Guid uid) : IRequest<VoterDto>;

public record VoterSimpleQuery(Guid uid) : IRequest<VoterDto>;
