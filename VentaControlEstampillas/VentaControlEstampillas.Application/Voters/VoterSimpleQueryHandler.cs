using VentaControlEstampillas.Domain.Dto;
using VentaControlEstampillas.Domain.Ports;
using MediatR;

namespace VentaControlEstampillas.Application.Voters;


//es un manejador para procesar consultas de tipo VoterSimpleQuery, recupera la informacion pertinente de un repositorio y devuelve un resultado de tipo VoterDto
public class VoterSimpleQueryHandler : IRequestHandler<VoterSimpleQuery, VoterDto>
{
    private readonly IVoterSimpleQueryRepository _repository;
    public VoterSimpleQueryHandler(IVoterSimpleQueryRepository repository) => _repository = repository;
    

    public async Task<VoterDto> Handle(VoterSimpleQuery request, CancellationToken cancellationToken)
    {
        return await _repository.Single(request.uid);                
    }
}