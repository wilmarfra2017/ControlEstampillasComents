using VentaControlEstampillas.Domain.Entities;
using VentaControlEstampillas.Domain.Ports;
using VentaControlEstampillas.Infrastructure.Ports;

namespace VentaControlEstampillas.Infrastructure.Adapters
{
    [Repository]
    public class VoterRepository : IVoterRepository
    {
        //_dataSource: Es una referencia a un repositorio genérico de tipo Voter. Este repositorio genérico es para operaciones de CRUD

        readonly IRepository<Voter> _dataSource;
        public VoterRepository(IRepository<Voter> dataSource) => _dataSource = dataSource 
            ?? throw new ArgumentNullException(nameof(dataSource));//Si el repositorio genérico proporcionado es nulo, se lanzará una excepción.        

        public async Task<Voter> SaveVoter(Voter v) => await _dataSource.AddAsync(v);

        public async Task<Voter> SingleVoter(Guid uid) => await _dataSource.GetOneAsync(uid);
        
        
    }
}

//En resumen, VoterRepository sirve como un adaptador entre la lógica del dominio y la infraestructura,
//permitiendo guardar y recuperar entidades Voter de una fuente de datos subyacente utilizando un repositorio genérico.