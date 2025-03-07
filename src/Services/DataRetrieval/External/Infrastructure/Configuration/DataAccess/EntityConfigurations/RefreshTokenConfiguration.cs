using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.DataAccess.EntityConfigurations;
public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_tokens", SchemasNames.Identity);

        builder.Property(r => r.Token).HasMaxLength(200);

        builder.HasIndex(r => r.Token).IsUnique();
    }
}
