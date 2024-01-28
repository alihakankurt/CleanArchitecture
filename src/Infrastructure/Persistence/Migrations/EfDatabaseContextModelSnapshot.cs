﻿// <auto-generated />
using System;
using Infrastructure.Persistence.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(EfDatabaseContext))]
    partial class EfDatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("Id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("CreatedAt");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("Email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("FirstName");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("LastName");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("BLOB")
                        .HasColumnName("PasswordHash");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("BLOB")
                        .HasColumnName("PasswordSalt");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("UpdatedAt");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.OwnsMany("Domain.Entities.RefreshToken", "RefreshTokens", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("INTEGER")
                                .HasColumnName("Id");

                            b1.Property<DateTime>("CreatedAt")
                                .HasColumnType("TEXT")
                                .HasColumnName("CreatedAt");

                            b1.Property<DateTime>("ExpiresAt")
                                .HasColumnType("TEXT")
                                .HasColumnName("ExpiresAt");

                            b1.Property<string>("IpAddress")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.Property<string>("Token")
                                .IsRequired()
                                .HasColumnType("TEXT")
                                .HasColumnName("Token");

                            b1.Property<int>("UserId")
                                .HasColumnType("INTEGER");

                            b1.HasKey("Id");

                            b1.HasIndex("UserId");

                            b1.ToTable("RefreshTokens", (string)null);

                            b1.WithOwner("User")
                                .HasForeignKey("UserId");

                            b1.Navigation("User");
                        });

                    b.Navigation("RefreshTokens");
                });
#pragma warning restore 612, 618
        }
    }
}
