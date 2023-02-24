﻿// <auto-generated />
using System;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccess.Migrations
{
    [DbContext(typeof(MoonBankContext))]
    [Migration("20230223221927_changes in CVU field format")]
    partial class changesinCVUfieldformat
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DataAccess.Models.Account", b =>
                {
                    b.Property<int>("IdAccount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdAccount"), 1L, 1);

                    b.Property<string>("Alias")
                        .IsRequired()
                        .HasColumnType("VARCHAR(50)");

                    b.Property<decimal?>("Balance")
                        .IsRequired()
                        .HasColumnType("DECIMAL(11,2)");

                    b.Property<string>("CBU_CVU")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("VARCHAR(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("VARCHAR(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("VARCHAR(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("VARCHAR(20)");

                    b.Property<int>("RewardPoints")
                        .HasColumnType("int");

                    b.HasKey("IdAccount");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("DataAccess.Models.Reward", b =>
                {
                    b.Property<int>("IdReward")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdReward"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("VARCHAR(50)");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.Property<string>("RewardUrlImage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdReward");

                    b.ToTable("Rewards");
                });

            modelBuilder.Entity("DataAccess.Models.Service", b =>
                {
                    b.Property<int>("IdService")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdService"), 1L, 1);

                    b.Property<decimal>("Amount")
                        .HasColumnType("DECIMAL(11,2)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("VARCHAR(50)");

                    b.Property<string>("ServiceUrlImage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdService");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("DataAccess.Models.Transaction", b =>
                {
                    b.Property<int>("IdTransaction")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdTransaction"), 1L, 1);

                    b.Property<decimal?>("Amount")
                        .HasColumnType("DECIMAL(11,2)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int?>("IdDestinationAccount")
                        .HasColumnType("int");

                    b.Property<int?>("IdReward")
                        .HasColumnType("int");

                    b.Property<int?>("IdService")
                        .HasColumnType("int");

                    b.Property<int?>("IdSourceAccount")
                        .HasColumnType("int");

                    b.Property<int?>("TypeDeposit")
                        .HasColumnType("int");

                    b.Property<int>("TypeTransaction")
                        .HasColumnType("int");

                    b.HasKey("IdTransaction");

                    b.HasIndex("IdDestinationAccount");

                    b.HasIndex("IdReward");

                    b.HasIndex("IdService");

                    b.HasIndex("IdSourceAccount");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("DataAccess.Models.Transaction", b =>
                {
                    b.HasOne("DataAccess.Models.Account", "DestinationAccount")
                        .WithMany()
                        .HasForeignKey("IdDestinationAccount")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("DataAccess.Models.Reward", "Reward")
                        .WithMany()
                        .HasForeignKey("IdReward")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("DataAccess.Models.Service", "Service")
                        .WithMany()
                        .HasForeignKey("IdService")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("DataAccess.Models.Account", "SourceAccount")
                        .WithMany()
                        .HasForeignKey("IdSourceAccount")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("DestinationAccount");

                    b.Navigation("Reward");

                    b.Navigation("Service");

                    b.Navigation("SourceAccount");
                });
#pragma warning restore 612, 618
        }
    }
}
