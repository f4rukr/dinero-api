﻿// <auto-generated />
using System;
using Klika.Dinero.Database.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Klika.Dinero.Database.Migrations
{
    [DbContext(typeof(DineroDbContext))]
    [Migration("20210728092821_dinero_seed_banks_and_categories")]
    partial class dinero_seed_banks_and_categories
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.8")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Klika.Dinero.Model.Entities.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccountNumber")
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<int>("BankId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CurrencyCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(3)");

                    b.Property<string>("IBAN")
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BankId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Klika.Dinero.Model.Entities.Bank", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Banks");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAt = new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(7458),
                            Description = "",
                            Name = "UniCredit Bank",
                            UpdatedAt = new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(7792)
                        },
                        new
                        {
                            Id = 2,
                            CreatedAt = new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8082),
                            Description = "",
                            Name = "Sberbank Bank",
                            UpdatedAt = new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8084)
                        },
                        new
                        {
                            Id = 3,
                            CreatedAt = new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8086),
                            Description = "",
                            Name = "Raiffeisen Bank",
                            UpdatedAt = new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8087)
                        },
                        new
                        {
                            Id = 4,
                            CreatedAt = new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8088),
                            Description = "",
                            Name = "Bosna Bank International",
                            UpdatedAt = new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8089)
                        },
                        new
                        {
                            Id = 5,
                            CreatedAt = new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8090),
                            Description = "",
                            Name = "Intesa Sanpaolo Bank",
                            UpdatedAt = new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8091)
                        },
                        new
                        {
                            Id = 6,
                            CreatedAt = new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8092),
                            Description = "",
                            Name = "NLB Bank",
                            UpdatedAt = new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8093)
                        },
                        new
                        {
                            Id = 7,
                            CreatedAt = new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8094),
                            Description = "",
                            Name = "Ziraat Banka",
                            UpdatedAt = new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8095)
                        },
                        new
                        {
                            Id = 8,
                            CreatedAt = new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8096),
                            Description = "",
                            Name = "Sparkasse Bank",
                            UpdatedAt = new DateTime(2021, 7, 28, 9, 28, 20, 761, DateTimeKind.Utc).AddTicks(8097)
                        });
                });

            modelBuilder.Entity("Klika.Dinero.Model.Entities.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,4)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Designation")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("TransactionCategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("TransactionCategoryId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("Klika.Dinero.Model.Entities.TransactionCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Keywords")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("TransactionCategories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAt = new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(1992),
                            Description = "",
                            Keywords = "",
                            Name = "Other",
                            UpdatedAt = new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2316)
                        },
                        new
                        {
                            Id = 2,
                            CreatedAt = new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2604),
                            Description = "",
                            Keywords = "ECOTOK;GAZPROM;HIFA BENZ;AUTOCESTE",
                            Name = "Vehicle",
                            UpdatedAt = new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2607)
                        },
                        new
                        {
                            Id = 3,
                            CreatedAt = new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2609),
                            Description = "",
                            Keywords = "KREDIT;TRAJNI NALOG;NAPLATA PO KRED.KARTICI",
                            Name = "Loan",
                            UpdatedAt = new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2610)
                        },
                        new
                        {
                            Id = 4,
                            CreatedAt = new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2611),
                            Description = "",
                            Keywords = "APOTEKA;PHARMACY;PHARM",
                            Name = "Pharmacy",
                            UpdatedAt = new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2616)
                        },
                        new
                        {
                            Id = 5,
                            CreatedAt = new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2617),
                            Description = "",
                            Keywords = "NAKNADA;TEKUCI RN KAMATA;PROVIZIJA ZA NALOG;PROVISION",
                            Name = "Bank fee",
                            UpdatedAt = new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2618)
                        },
                        new
                        {
                            Id = 6,
                            CreatedAt = new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2619),
                            Description = "",
                            Keywords = "ATM",
                            Name = "ATM",
                            UpdatedAt = new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2620)
                        },
                        new
                        {
                            Id = 7,
                            CreatedAt = new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2624),
                            Description = "",
                            Keywords = "DOO;KUPOVINA;SHOPPING;BINGO;KONZUM;MERKATOR",
                            Name = "Shopping",
                            UpdatedAt = new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2625)
                        },
                        new
                        {
                            Id = 8,
                            CreatedAt = new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2664),
                            Description = "",
                            Keywords = "PLATA;TOPL;PREVOZ;UPLATA",
                            Name = "Salary",
                            UpdatedAt = new DateTime(2021, 7, 28, 9, 28, 20, 763, DateTimeKind.Utc).AddTicks(2665)
                        });
                });

            modelBuilder.Entity("Klika.Dinero.Model.Entities.Account", b =>
                {
                    b.HasOne("Klika.Dinero.Model.Entities.Bank", "Bank")
                        .WithMany("Accounts")
                        .HasForeignKey("BankId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bank");
                });

            modelBuilder.Entity("Klika.Dinero.Model.Entities.Transaction", b =>
                {
                    b.HasOne("Klika.Dinero.Model.Entities.Account", "Account")
                        .WithMany("Transactions")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Klika.Dinero.Model.Entities.TransactionCategory", "TransactionCategory")
                        .WithMany("Transactions")
                        .HasForeignKey("TransactionCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("TransactionCategory");
                });

            modelBuilder.Entity("Klika.Dinero.Model.Entities.Account", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("Klika.Dinero.Model.Entities.Bank", b =>
                {
                    b.Navigation("Accounts");
                });

            modelBuilder.Entity("Klika.Dinero.Model.Entities.TransactionCategory", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}