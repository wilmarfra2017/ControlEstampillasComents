using VentaControlEstampillas.Domain.Dto;
using MediatR;


namespace VentaControlEstampillas.Application.Voters;

public record VoterRegisterCommand(string Nid, string Origin, DateTime Dob) : IRequest<VoterDto>;

//Un record en C# es una clase especial que no permite
//cambios después de su creación; en lugar de modificarla, se crea una nueva copia con los cambios.

//Ser inmutable significa que una vez que se crea una instancia de un record, no puede ser modificada.
//En lugar de modificarlo, cualquier operación que cambie algún valor produce una nueva instancia del record con el valor actualizado.