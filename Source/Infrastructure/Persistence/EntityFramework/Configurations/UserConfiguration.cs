using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityFramework.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users")
            .HasKey(e => e.Id);

        builder.Property(e => e.Id).ValueGeneratedNever().IsRequired();
        builder.Property(e => e.FirstName).IsRequired();
        builder.Property(e => e.LastName).IsRequired();
        builder.Property(e => e.Email).IsRequired();
        builder.Property(e => e.PasswordHash).HasMaxLength(64).IsRequired();
        builder.Property(e => e.PasswordSalt).HasMaxLength(128).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.UpdatedAt).IsRequired();

        builder.OwnsMany(e => e.RefreshTokens, static (builder) =>
        {
            builder.ToTable("RefreshTokens")
                .HasKey(e => new { e.UserId, e.Token });

            builder.WithOwner().HasForeignKey(e => e.UserId);

            builder.Property(e => e.UserId).IsRequired();
            builder.Property(e => e.Token).IsRequired();
            builder.Property(e => e.IssuedAt).IsRequired();
            builder.Property(e => e.ExpiresAt).IsRequired();
            builder.Property(e => e.IsRevoked).IsRequired();

            builder.HasIndex(e => e.Token);
        });

        builder.Navigation(e => e.RefreshTokens)
            .HasField("_refreshTokens")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .AutoInclude();
    }
}
