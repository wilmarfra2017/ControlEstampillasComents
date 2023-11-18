using VentaControlEstampillas.Domain.Dto;
using VentaControlEstampillas.Domain.Entities;
using VentaControlEstampillas.Domain.Services;
using MediatR;


namespace VentaControlEstampillas.Application.Voters;

public class VoterRegisterCommandHandler : IRequestHandler<VoterRegisterCommand, VoterDto>
{
    private readonly RecordVoterService _service;

    public VoterRegisterCommandHandler(RecordVoterService service) =>
        _service = service ?? throw new ArgumentNullException(nameof(service));
    //si el service es nulo lanza una excepcion, esto asegura que siempre se proporcione un servicio valido cuando se cree una instancia de VoterRegisterCommandHandler.

    public async Task<VoterDto> Handle(VoterRegisterCommand request, CancellationToken cancellationToken)
    {
        //invoca al método _service.RecordVoterAsync, al que se le pasa una nueva instancia de Voter creada a partir de los datos en el comando.

        var voterSaved =   await _service.RecordVoterAsync(
            new Voter(request.Nid, request.Dob, request.Origin), cancellationToken
        );

        //Una vez que se registra y guarda el votante, se devuelve un VoterDto con los detalles del votante registrado

        return new VoterDto(voterSaved.Id, voterSaved.DateOfBirth, voterSaved.Origin);
    }
}
