using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfig : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> b)
    {
        b.ToTable("RefreshTokens");

        b.HasKey(x => x.Id);

        b.Property(x => x.TokenHash)
            .HasMaxLength(255)
            .IsRequired();

        b.HasIndex(x => x.TokenHash);

        b.Property(x => x.CreatedAtUtc).IsRequired();
        b.Property(x => x.ExpiresAtUtc).IsRequired();
    }
}