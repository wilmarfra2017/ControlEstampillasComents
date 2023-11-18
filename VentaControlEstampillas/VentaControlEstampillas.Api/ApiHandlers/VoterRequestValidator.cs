using VentaControlEstampillas.Application.Voters;
using FluentValidation;

namespace VentaControlEstampillas.Api.ApiHandlers;

/*
 * la clase VoterRequestValidator usa FluentValidation para definir 
 * reglas espec�ficas que un objeto VoterRegisterCommand
 * Es probable que este validador se use en alg�n lugar donde se intenten registrar nuevos votantes para asegurarse 
 * de que los datos proporcionados son v�lidos antes de procesarlos o guardarlos.
 */

public class VoterRequestValidator : AbstractValidator<VoterRegisterCommand>
{
    const int MIN_LENGTH = 8;
    public VoterRequestValidator()
    {
        RuleFor(x => x.Nid).NotEmpty().MinimumLength(MIN_LENGTH);
        RuleFor(x => x.Dob).NotEmpty();
        RuleFor(x => x.Origin).NotEmpty();
    }
}