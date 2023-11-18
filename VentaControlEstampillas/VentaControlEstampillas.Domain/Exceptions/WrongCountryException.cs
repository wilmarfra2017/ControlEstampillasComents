using System.Runtime.Serialization;

namespace VentaControlEstampillas.Domain.Exceptions;

[Serializable]
public sealed class WrongCountryException : CoreBusinessException
{
    public WrongCountryException()
    {
    }
    public WrongCountryException(string msg) : base(msg)
    {
    }

    public WrongCountryException(string message, Exception inner) : base(message, inner)
    {
    }

    //Constructor privado para serialización: Es utilizado durante el proceso de deserialización para reconstruir la excepción de su representación serializada.
    private WrongCountryException(SerializationInfo info, StreamingContext context)
   : base(info, context)
    {
    }

}

//En resumen, WrongCountryException es una excepción personalizada utilizada para representar errores específicos relacionados con problemas de país en el dominio de la aplicación. Al extender CoreBusinessException,
//hereda todas sus características y agrega una semántica más específica al tipo de error que representa.
