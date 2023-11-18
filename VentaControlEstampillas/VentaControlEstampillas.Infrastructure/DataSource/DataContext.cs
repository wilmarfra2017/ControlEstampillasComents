using VentaControlEstampillas.Domain.Entities;
using VentaControlEstampillas.Infrastructure.DataSource.ModelConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace VentaControlEstampillas.Infrastructure.DataSource;

public class DataContext : DbContext
{
    private readonly IConfiguration _config;
    public DataContext(DbContextOptions<DataContext> options, IConfiguration config) : base(options)
    {
        _config = config;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (modelBuilder is null)
        {
            return;
        }

        // cargar toda la configuración de entidad del ensamblado actual
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

        // si se utiliza esquema en la base de datos, descomente la siguiente línea
        // modelBuilder.HasDefaultSchema(_config.GetValue<string>("SchemaName"));
        modelBuilder.Entity<Voter>();

        // propiedades fantasma para auditoría
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var t = entityType.ClrType;
            if (typeof(DomainEntity).IsAssignableFrom(t))
            {
                modelBuilder.Entity(entityType.Name).Property<DateTime>("CreatedOn");
                modelBuilder.Entity(entityType.Name).Property<DateTime>("LastModifiedOn");
            }
        }

        base.OnModelCreating(modelBuilder);
    }
}
