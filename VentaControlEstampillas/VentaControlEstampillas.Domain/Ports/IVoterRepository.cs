using VentaControlEstampillas.Domain.Entities;

namespace VentaControlEstampillas.Domain.Ports
{
    public interface IVoterRepository
    {
        Task<Voter> SaveVoter(Voter v);     
        Task<Voter> SingleVoter(Guid uid);   
        
    }

   
}

