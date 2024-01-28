using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.EntityFramework.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users").HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("Id").IsRequired();
        builder.Property(x => x.FirstName).HasColumnName("FirstName").IsRequired();
        builder.Property(x => x.LastName).HasColumnName("LastName").IsRequired();
        builder.Property(x => x.Email).HasColumnName("Email").IsRequired();
        builder.Property(x => x.PasswordHash).HasColumnName("PasswordHash").HasMaxLength(64).IsRequired();
        builder.Property(x => x.PasswordSalt).HasColumnName("PasswordSalt").HasMaxLength(128).IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("CreatedAt").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("UpdatedAt").IsRequired();

        builder.Ignore(x => x.FullName);

        builder.OwnsMany(x => x.RefreshTokens, static (builder) =>
        {
            builder.ToTable("RefreshTokens").HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("Id").IsRequired();
            builder.Property(x => x.Token).HasColumnName("Token").IsRequired();
            builder.Property(x => x.ExpiresAt).HasColumnName("ExpiresAt").IsRequired();
            builder.Property(x => x.CreatedAt).HasColumnName("CreatedAt").IsRequired();
        });

        builder.Navigation(x => x.RefreshTokens).AutoInclude();
    }
}
