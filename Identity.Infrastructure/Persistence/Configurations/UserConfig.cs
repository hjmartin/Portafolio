using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Persistence.Configurations
{
    public class UserConfig: IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> b)
        {
            b.ToTable("Users");

            b.HasKey(x => x.Id);

            b.Property(x => x.Email)
                .HasMaxLength(150)
                .IsRequired();

            b.HasIndex(x => x.Email)
                .IsUnique();

            b.Property(x => x.PasswordHash)
                .HasMaxLength(255)
                .IsRequired();

            b.Property(x => x.FullName)
                .HasMaxLength(150);

            b.Property(x => x.IsActive)
                .IsRequired();

            b.Property(x => x.CreatedAtUtc)
                .IsRequired();

            // Relaciones
            b.HasMany(x => x.UserRoles)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasMany(x => x.RefreshTokens)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
