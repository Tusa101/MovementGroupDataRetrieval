using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.DataAccess.EntityConfigurations;
public class StoredDataConfiguration : IEntityTypeConfiguration<StoredData>
{
    public void Configure(EntityTypeBuilder<StoredData> builder)
    {
        builder.ToTable("stored_data", SchemasNames.DataRetrieval);
    }
}
