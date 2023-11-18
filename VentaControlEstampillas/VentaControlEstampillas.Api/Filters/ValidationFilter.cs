using FluentValidation;
using System.Net;
using System.Reflection;

namespace VentaControlEstampillas.Api.Filters;

/*
 * En resumen, esta implementación proporciona una forma de validar automáticamente los argumentos de los puntos finales de la API usando FluentValidation. 
 * Si un argumento tiene asociado el atributo ValidateAttribute, se espera que haya un validador FluentValidation registrado que pueda validar ese tipo de argumento. 
 * Si la validación falla, se devuelve una respuesta con un código de estado UnprocessableEntity (422).
 */


//se puede usar parametros y no se permite aplicar el mismo atributo más de una vez al mismo parámetro
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
public class ValidateAttribute : Attribute
{
}

public static class ValidationFilter
{

    /*
     * Método ValidationFilterFactory:

      Este método crea y devuelve un delegado que puede ser utilizado para filtrar y validar los argumentos de un punto final.
      Obtiene todos los validadores asociados con el punto final y, si hay alguno, devuelve un delegado que invocará el método de validación ValidateAsync. 
      Si no hay validadores, simplemente devuelve un delegado que pasa al siguiente filtro o manejador.     
    */


    public static EndpointFilterDelegate ValidationFilterFactory(EndpointFilterFactoryContext context, EndpointFilterDelegate next)
    {
        IEnumerable<ValidationDescriptor> validationDescriptors = GetValidators(context.MethodInfo, context.ApplicationServices);

        if (validationDescriptors.Any())
        {
            //delegado = invocationContext
            return invocationContext => ValidateAsync(validationDescriptors, invocationContext, next);
        }

        // pass-thru
        return invocationContext => next(invocationContext);
    }


    /*
     * Este método itera sobre todos los validadores, valida cada argumento y, si encuentra algún problema de validación, 
     * devuelve un resultado ValidationProblem con un código de estado UnprocessableEntity (422). 
     * Si todos los argumentos son válidos, pasa al siguiente filtro o manejador.      
     */
    private static async ValueTask<object?> ValidateAsync(IEnumerable<ValidationDescriptor> validationDescriptors, EndpointFilterInvocationContext invocationContext, EndpointFilterDelegate next)
    {
        foreach (ValidationDescriptor descriptor in validationDescriptors)
        {
            var argument = invocationContext.Arguments[descriptor.ArgumentIndex];

            if (argument is not null)
            {
                var validationResult = await descriptor.Validator.ValidateAsync(
                    new ValidationContext<object>(argument)
                );

                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary(),
                        statusCode: (int)HttpStatusCode.UnprocessableEntity);
                }
            }
        }

        return await next.Invoke(invocationContext);
    }

    /*
     * Este método busca parámetros en un método que tengan el atributo ValidateAttribute y, para aquellos que lo tienen, busca un validador FluentValidation asociado.
       Nota: El comentario menciona que los validadores FluentValidation deben estar registrados como singleton. Esto es una indicación de que se espera que los validadores 
       se registren en el contenedor de inyección de dependencias del proyecto como singletons.       
    */

    static IEnumerable<ValidationDescriptor> GetValidators(MethodBase methodInfo, IServiceProvider serviceProvider)
    {
        ParameterInfo[] parameters = methodInfo.GetParameters();

        for (int i = 0; i < parameters.Length; i++)
        {
            ParameterInfo parameter = parameters[i];

            if (parameter.GetCustomAttribute<ValidateAttribute>() is not null)
            {
                Type validatorType = typeof(IValidator<>).MakeGenericType(parameter.ParameterType);

                // Tenga en cuenta que los validadores de FluentValidation deben registrarse como singleton
                IValidator? validator = serviceProvider.GetService(validatorType) as IValidator;

                if (validator is not null)
                {
                    yield return new ValidationDescriptor { ArgumentIndex = i, ArgumentType = parameter.ParameterType, Validator = validator };
                }
            }
        }
    }

    private class ValidationDescriptor
    {
        public required int ArgumentIndex { get; init; }
        public required Type ArgumentType { get; init; }
        public required IValidator Validator { get; init; }
    }
}
