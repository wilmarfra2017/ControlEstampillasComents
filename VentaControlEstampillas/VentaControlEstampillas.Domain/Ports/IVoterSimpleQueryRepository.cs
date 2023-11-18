using VentaControlEstampillas.Domain.Dto;

namespace VentaControlEstampillas.Domain.Ports
{
    public interface IVoterSimpleQueryRepository
    {
        Task<VoterDto> Single(Guid id);
    }
}

