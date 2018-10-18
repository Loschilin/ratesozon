﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RateReader.Data.Contexts;

namespace RateReader.Data.Migrations
{
    [DbContext(typeof(DbCurrencyContext))]
    [Migration("20181018202104_InitalCreate")]
    partial class InitalCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RateReader.Data.Entities.DbCurrency", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("getdate()");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique()
                        .HasFilter("[Code] IS NOT NULL");

                    b.ToTable("Currencies");
                });

            modelBuilder.Entity("RateReader.Data.Entities.DbCurrencyValue", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Amount");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("getdate()");

                    b.Property<Guid>("CurrencyId");

                    b.Property<DateTime>("Date");

                    b.Property<decimal>("Rate")
                        .HasColumnType("decimal(7,3)");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.HasIndex("CurrencyId", "Date")
                        .IsUnique();

                    b.ToTable("CurrencyValues");
                });

            modelBuilder.Entity("RateReader.Data.Entities.DbCurrencyValue", b =>
                {
                    b.HasOne("RateReader.Data.Entities.DbCurrency", "Currency")
                        .WithMany("Values")
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
