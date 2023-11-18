using VentaControlEstampillas.Domain.Dto;
using VentaControlEstampillas.Domain.Ports;
using MediatR;

namespace VentaControlEstampillas.Application.Voters;

/*
 * VoterQueryHandler es un manejador de MediatR diseñado para procesar consultas de tipo VoterQuery, 
 * recuperar la información pertinente de un repositorio y devolver un resultado de tipo VoterDto.
 */
public class VoterQueryHandler : IRequestHandler<VoterQuery, VoterDto>
{
    private readonly IVoterRepository _repository;
    public VoterQueryHandler(IVoterRepository repository) => _repository = repository;
    

    public async Task<VoterDto> Handle(VoterQuery request, CancellationToken cancellationToken)
    {
        //define como se va a manejar la consulta

        var voter = await _repository.SingleVoter(request.uid);        
        return new VoterDto(voter.Id, voter.DateOfBirth, voter.Origin);
    }
}