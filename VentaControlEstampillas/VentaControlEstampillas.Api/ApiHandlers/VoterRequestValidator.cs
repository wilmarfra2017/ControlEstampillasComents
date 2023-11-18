using VentaControlEstampillas.Application.Voters;
using FluentValidation;

namespace VentaControlEstampillas.Api.ApiHandlers;

/*
 * la clase VoterRequestValidator usa FluentValidation para definir 
 * reglas específicas que un objeto VoterRegisterCommand
 * Es probable que este validador se use en algún lugar donde se intenten registrar nuevos votantes para asegurarse 
 * de que los datos proporcionados son válidos antes de procesarlos o guardarlos.
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