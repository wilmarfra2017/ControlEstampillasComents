using VentaControlEstampillas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VentaControlEstampillas.Infrastructure.DataSource.ModelConfig;

public class VoterEntityTypeConfiguration : IEntityTypeConfiguration<Voter>
{
    // Si necesitamos db constrains, este es el lugar 
    public void Configure(EntityTypeBuilder<Voter> builder)
    {
        builder.Property(b => b.Nid).IsRequired();
    }
}

