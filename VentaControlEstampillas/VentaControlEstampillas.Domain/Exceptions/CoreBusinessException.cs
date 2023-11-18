using System.Runtime.Serialization;
namespace VentaControlEstampillas.Domain.Exceptions;

[Serializable]

//La clase CoreBusinessException hereda de la clase base Exception, lo que significa que es una excepción y puede ser capturada en bloques try-catch.
public class CoreBusinessException : Exception
{
    public CoreBusinessException()
    {
        //Este es el constructor predeterminado sin parametros. Crea una nueva instancia de la excepción sin un mensaje específico.
    }
    public CoreBusinessException(string message) : base(message)
    {
        //Constructor con mensaje: Acepta un mensaje de error y lo pasa al constructor de la clase base (Exception).
    }

    public CoreBusinessException(string message, Exception inner) : base(message, inner)
    {
        //Constructor con mensaje e inner exception: Acepta tanto un mensaje de error como una excepción interna (otra excepción que causó esta excepción).
        //Esta sobrecarga es útil para "envolver" excepciones de niveles inferiores mientras se proporciona información adicional.
    }


    //Constructor protegido para serialización: Este constructor se usa durante el proceso de
    //deserialización para reconstruir la excepción a partir de su representación serializada.
    protected CoreBusinessException(SerializationInfo info, StreamingContext context) 
    : base(info, context) 
    {        
    }
}

//En resumen, CoreBusinessException es una excepción personalizada que se podría usar para indicar errores en la lógica de negocio
//del dominio de la aplicación. Al tener múltiples constructores, ofrece flexibilidad en cómo se crea y maneja la excepción.