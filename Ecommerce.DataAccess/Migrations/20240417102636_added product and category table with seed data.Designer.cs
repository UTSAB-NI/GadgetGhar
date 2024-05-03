﻿// <auto-generated />
using System;
using Ecommerce.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Ecommerce.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240417102636_added product and category table with seed data")]
    partial class addedproductandcategorytablewithseeddata
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0-preview.3.24172.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Ecommerce.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAt = new DateTime(2024, 3, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Laptop",
                            UpdatedAt = new DateTime(2023, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 2,
                            CreatedAt = new DateTime(2024, 3, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Mobile",
                            UpdatedAt = new DateTime(2023, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 3,
                            CreatedAt = new DateTime(2024, 3, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Headphones",
                            UpdatedAt = new DateTime(2023, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 4,
                            CreatedAt = new DateTime(2024, 3, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Accessories",
                            UpdatedAt = new DateTime(2023, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("Ecommerce.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,4)");

                    b.Property<int>("StockQuantity")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryId = 0,
                            CreatedAt = new DateTime(2023, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Closed [endoscopic] biopsy of small intestine",
                            ImageUrl = "http://dummyimage.com/166x100.png/cc0000/ffffff",
                            Name = "Banana Turning",
                            Price = 483m,
                            StockQuantity = 1,
                            UpdatedAt = new DateTime(2024, 3, 19, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 2,
                            CategoryId = 0,
                            CreatedAt = new DateTime(2023, 8, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Narcoanalysis",
                            ImageUrl = "http://dummyimage.com/231x100.png/cc0000/ffffff",
                            Name = "Bay Leaf Fresh",
                            Price = 1335m,
                            StockQuantity = 2,
                            UpdatedAt = new DateTime(2023, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 3,
                            CategoryId = 0,
                            CreatedAt = new DateTime(2023, 10, 31, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Arthrotomy for removal of prosthesis without replacement, elbow",
                            ImageUrl = "http://dummyimage.com/218x100.png/5fa2dd/ffffff",
                            Name = "Sprouts - Brussel",
                            Price = 3789m,
                            StockQuantity = 3,
                            UpdatedAt = new DateTime(2023, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("Ecommerce.Models.Product", b =>
                {
                    b.HasOne("Ecommerce.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });
#pragma warning restore 612, 618
        }
    }
}