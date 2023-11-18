using VentaControlEstampillas.Domain.Entities;
using VentaControlEstampillas.Domain.Exceptions;
using VentaControlEstampillas.Domain.Ports;

namespace VentaControlEstampillas.Domain.Services;

[DomainService]
public class RecordVoterService
{
    private readonly IVoterRepository _voterRepository;
    private readonly IUnitOfWork _unitOfWork;
    const string VOTER_ORIGIN = "Colombia";

    public RecordVoterService(IVoterRepository voterRepository, IUnitOfWork unitOfWork) => 
        (_voterRepository, _unitOfWork) = (voterRepository, unitOfWork);                    

    public async Task<Voter> RecordVoterAsync(Voter v, CancellationToken? cancellationToken = null)
    {       
        var token = cancellationToken ?? new CancellationTokenSource().Token;       
        CheckOrigin(v);
        CheckAge(v);
        var returnVote = await _voterRepository.SaveVoter(v);
        await _unitOfWork.SaveAsync(token);
        return returnVote;
    }

    void CheckAge(Voter v)
    {
        if (v.IsUnderAge)
        {
            throw new UnderAgeException("Voter is not 18 years or older");
        }
    }

    void CheckOrigin(Voter v)
    {
        if (!v.CanVoteBasedOnLocation)
        {
            throw new WrongCountryException($"Voter is not from {VOTER_ORIGIN}");
        }
    }
}
