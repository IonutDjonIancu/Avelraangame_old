﻿// <auto-generated />
using System;
using Avelraangame.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Avelraangame.Migrations
{
    [DbContext(typeof(AvelraanContext))]
    [Migration("20210627200505_changed_isDraft_to_hasLevelUp")]
    partial class changed_isDraft_to_hasLevelUp
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:Collation", "Latin1_General_CI_AS")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Avelraangame.Models.Character", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Abstract")
                        .HasColumnType("int");

                    b.Property<int>("Awareness")
                        .HasColumnType("int");

                    b.Property<int>("Culture")
                        .HasColumnType("int");

                    b.Property<int>("DRM")
                        .HasColumnType("int");

                    b.Property<int>("EntityLevel")
                        .HasColumnType("int");

                    b.Property<string>("Equippment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Experience")
                        .HasColumnType("int");

                    b.Property<int>("Harm")
                        .HasColumnType("int");

                    b.Property<bool>("HasLevelup")
                        .HasColumnType("bit");

                    b.Property<int>("Health")
                        .HasColumnType("int");

                    b.Property<string>("HeroicTraits")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("InParty")
                        .HasColumnType("bit");

                    b.Property<bool>("IsAlive")
                        .HasColumnType("bit");

                    b.Property<string>("Logbook")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Mana")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("NegativePerks")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("PartyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("PlayerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Race")
                        .HasColumnType("int");

                    b.Property<int>("Strength")
                        .HasColumnType("int");

                    b.Property<string>("Supplies")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Toughness")
                        .HasColumnType("int");

                    b.Property<int>("Wealth")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PartyId");

                    b.HasIndex("PlayerId");

                    b.ToTable("Characters");
                });

            modelBuilder.Entity("Avelraangame.Models.HeroicTraits", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Bonuses")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("HeroicTraits");
                });

            modelBuilder.Entity("Avelraangame.Models.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Bonuses")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("CharacterId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("InSlot")
                        .HasColumnType("int");

                    b.Property<bool>("IsConsumable")
                        .HasColumnType("bit");

                    b.Property<bool>("IsEquipped")
                        .HasColumnType("bit");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Slots")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int>("Worth")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("Avelraangame.Models.NegativePerks", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Bonuses")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("NegativePerks");
                });

            modelBuilder.Entity("Avelraangame.Models.Party", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Party");
                });

            modelBuilder.Entity("Avelraangame.Models.Player", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastLogin")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Ward")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Avelraangame.Models.TempInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("BonusTo")
                        .HasColumnType("int");

                    b.Property<Guid>("CharacterId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CharacterId");

                    b.ToTable("Temps");
                });

            modelBuilder.Entity("Avelraangame.Models.Character", b =>
                {
                    b.HasOne("Avelraangame.Models.Party", "Party")
                        .WithMany("Characters")
                        .HasForeignKey("PartyId");

                    b.HasOne("Avelraangame.Models.Player", "Player")
                        .WithMany("Characters")
                        .HasForeignKey("PlayerId");

                    b.Navigation("Party");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Avelraangame.Models.TempInfo", b =>
                {
                    b.HasOne("Avelraangame.Models.Character", "Character")
                        .WithMany("Temps")
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Character");
                });

            modelBuilder.Entity("Avelraangame.Models.Character", b =>
                {
                    b.Navigation("Temps");
                });

            modelBuilder.Entity("Avelraangame.Models.Party", b =>
                {
                    b.Navigation("Characters");
                });

            modelBuilder.Entity("Avelraangame.Models.Player", b =>
                {
                    b.Navigation("Characters");
                });
#pragma warning restore 612, 618
        }
    }
}
