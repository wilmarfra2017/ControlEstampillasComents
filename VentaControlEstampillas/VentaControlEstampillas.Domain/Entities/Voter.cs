using VentaControlEstampillas.Domain.Exceptions;


namespace VentaControlEstampillas.Domain.Entities;

public class Voter : DomainEntity
{
    const int MINIMUM_AGE = 18;
    const string COUNTRY_OF_ORIGIN = "COLOMBIA";

    // Constructor no vacio
    public Voter(string nid, DateTime dateOfBirth, string origin)
    {
        Nid = nid.Length >= 8 ? nid : throw new CoreBusinessException("el documento requiere al menos 8 caracteres");
        DateOfBirth = dateOfBirth;
        Origin = origin;
    }

    // menor de edad ?
    public bool IsUnderAge => ((new DateTime((DateTime.Now - this.DateOfBirth).Ticks).Year) - 1) < MINIMUM_AGE;

    // lugar permitido para votar ?
    public bool CanVoteBasedOnLocation => this.Origin.ToUpper(System.Globalization.CultureInfo.InvariantCulture) == COUNTRY_OF_ORIGIN;


    // Private setter, if we require inmutability for the entity fields. These values we get a construction time.    
    
    public string Nid { get; init; }
    public DateTime DateOfBirth { get; init; }
    public string Origin { get; init; }
}

