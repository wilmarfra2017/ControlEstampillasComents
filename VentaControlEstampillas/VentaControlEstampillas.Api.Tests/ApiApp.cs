
using VentaControlEstampillas.Infrastructure.DataSource;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace VentaControlEstampillas.Api.Tests;

class ApiApp : WebApplicationFactory<Program>
{

    readonly Guid _id;

    public Guid UserId => this._id;

    public ApiApp()
    {
        _id = Guid.NewGuid();
    }

    // We should use this service collection to access repos and seed data for tests
    public IServiceProvider GetServiceCollection()
    {
        return Services;           
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(svc =>
        {
            svc.RemoveAll(typeof(DbContextOptions<DataContext>));
            svc.AddDbContext<DataContext>(opt =>
            {
                opt.UseInMemoryDatabase("testdb");
            });

        });

        return base.CreateHost(builder);
    }

    
}

