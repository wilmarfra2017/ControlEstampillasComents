using System.Runtime.Serialization;

namespace VentaControlEstampillas.Domain.Exceptions;

[Serializable]
public sealed class UnderAgeException : CoreBusinessException
{
    public UnderAgeException()
    {        
    }
    
    public UnderAgeException(string msg) : base(msg)
    {
    }

    public UnderAgeException(string message, Exception inner) : base(message, inner)
    {
    }

    private  UnderAgeException(SerializationInfo info, StreamingContext context) 
    : base(info, context) 
    {        
    }

}

//En esencia, UnderAgeException es una excepción personalizada utilizada para representar errores específicos relacionados
//con problemas de edad mínima en el dominio de la aplicación.
//Al extender CoreBusinessException, hereda todas sus características y agrega una semántica más específica.