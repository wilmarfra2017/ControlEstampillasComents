namespace VentaControlEstampillas.Domain.Ports;
public interface IUnitOfWork
{
    Task SaveAsync(CancellationToken? cancellationToken = null);
}
