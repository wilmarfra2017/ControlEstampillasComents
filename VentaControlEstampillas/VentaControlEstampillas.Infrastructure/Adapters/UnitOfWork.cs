using VentaControlEstampillas.Domain.Ports;
using VentaControlEstampillas.Infrastructure.DataSource;
using Microsoft.EntityFrameworkCore;

namespace VentaControlEstampillas.Infrastructure.Adapters;

public class UnitOfWork : IUnitOfWork
{

    private readonly DataContext _context;
    public UnitOfWork(DataContext context)
    {
        _context = context;
    }
    public async Task SaveAsync(CancellationToken? cancellationToken = null)
    {
        var token = cancellationToken ?? new CancellationTokenSource().Token;

        _context.ChangeTracker.DetectChanges();
        
        var entryStatus = new Dictionary<EntityState, string> {
            {EntityState.Added, "CreatedOn"},
            {EntityState.Modified, "LastModifiedOn"}
        };

        foreach (var entry in _context.ChangeTracker.Entries())
        {
            if (entryStatus.ContainsKey(entry.State)) {
                entry.Property(entryStatus[entry.State]).CurrentValue = DateTime.UtcNow;
            }           
        }

        await _context.SaveChangesAsync(token);
    }
}


//En resumen, esta clase UnitOfWork proporciona una forma de gestionar y persistir los cambios realizados
//a las entidades en una operación de negocio. Cuando se invoca el método SaveAsync, la clase se asegura de
//que todos los cambios se detecten, actualiza las propiedades de auditoría de las entidades y
//luego guarda todos los cambios en la base de datos. La ventaja de usar una Unidad de Trabajo es que encapsula
//la lógica de persistencia, proporcionando una capa de abstracción entre la lógica de negocio y la infraestructura de persistencia.