using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityFramework.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users").HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("Id").IsRequired();
        builder.Property(e => e.FirstName).HasColumnName("FirstName").IsRequired();
        builder.Property(e => e.LastName).HasColumnName("LastName").IsRequired();
        builder.Property(e => e.Email).HasColumnName("Email").IsRequired();
        builder.Property(e => e.PasswordHash).HasColumnName("PasswordHash").HasMaxLength(64).IsRequired();
        builder.Property(e => e.PasswordSalt).HasColumnName("PasswordSalt").HasMaxLength(128).IsRequired();
        builder.Property(e => e.CreatedAt).HasColumnName("CreatedAt").IsRequired();
        builder.Property(e => e.UpdatedAt).HasColumnName("UpdatedAt").IsRequired();

        builder.Ignore(e => e.FullName);

        builder.OwnsMany(e => e.RefreshTokens, static (builder) =>
        {
            builder.ToTable("RefreshTokens").HasKey(e => new { e.UserId, e.Token });

            builder.Property(e => e.UserId).HasColumnName("UserId").IsRequired();
            builder.Property(e => e.Token).HasColumnName("Token").IsRequired();
            builder.Property(e => e.CreatedAt).HasColumnName("CreatedAt").IsRequired();
            builder.Property(e => e.ExpiresAt).HasColumnName("ExpiresAt").IsRequired();
        });

        builder.Navigation(e => e.RefreshTokens).AutoInclude();
    }
}
