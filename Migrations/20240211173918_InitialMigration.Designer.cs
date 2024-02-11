﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using maturitetna.Data;

#nullable disable

namespace maturitetna.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240211173918_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("maturitetna.Models.adminEntity", b =>
                {
                    b.Property<int>("id_admin")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("created")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("lastname")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("id_admin");

                    b.ToTable("admin");
                });

            modelBuilder.Entity("maturitetna.Models.appointmentEntity", b =>
                {
                    b.Property<int>("id_appointment")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("appointmentTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("id_haircut")
                        .HasColumnType("int");

                    b.Property<int>("id_hairdresser")
                        .HasColumnType("int");

                    b.Property<int>("id_user")
                        .HasColumnType("int");

                    b.HasKey("id_appointment");

                    b.HasIndex("id_haircut");

                    b.HasIndex("id_hairdresser");

                    b.HasIndex("id_user");

                    b.ToTable("appointment");
                });

            modelBuilder.Entity("maturitetna.Models.haircutEntity", b =>
                {
                    b.Property<int>("id_haircut")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("duration")
                        .HasColumnType("int");

                    b.Property<int?>("hairdresserEntityid_hairdresser")
                        .HasColumnType("int");

                    b.Property<string>("image")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("style")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("id_haircut");

                    b.HasIndex("hairdresserEntityid_hairdresser");

                    b.ToTable("haircut");
                });

            modelBuilder.Entity("maturitetna.Models.hairdresserEntity", b =>
                {
                    b.Property<int>("id_hairdresser")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("created")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<TimeSpan>("endTime")
                        .HasColumnType("time(6)");

                    b.Property<int?>("gender")
                        .HasColumnType("int");

                    b.Property<bool>("is_working")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("lastname")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<TimeSpan>("startTime")
                        .HasColumnType("time(6)");

                    b.HasKey("id_hairdresser");

                    b.ToTable("hairdresser");
                });

            modelBuilder.Entity("maturitetna.Models.hairdresserHaircutEntity", b =>
                {
                    b.Property<int>("id_hairdresser")
                        .HasColumnType("int");

                    b.Property<int>("id_haircut")
                        .HasColumnType("int");

                    b.HasKey("id_hairdresser", "id_haircut");

                    b.HasIndex("id_haircut");

                    b.ToTable("hairdresserHaircut");
                });

            modelBuilder.Entity("maturitetna.Models.userEntity", b =>
                {
                    b.Property<int>("id_user")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("created")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("lastname")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("id_user");

                    b.ToTable("user");
                });

            modelBuilder.Entity("maturitetna.Models.appointmentEntity", b =>
                {
                    b.HasOne("maturitetna.Models.haircutEntity", "haircut")
                        .WithMany()
                        .HasForeignKey("id_haircut")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("maturitetna.Models.hairdresserEntity", "hairdresser")
                        .WithMany("appointements")
                        .HasForeignKey("id_hairdresser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("maturitetna.Models.userEntity", "user")
                        .WithMany()
                        .HasForeignKey("id_user")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("haircut");

                    b.Navigation("hairdresser");

                    b.Navigation("user");
                });

            modelBuilder.Entity("maturitetna.Models.haircutEntity", b =>
                {
                    b.HasOne("maturitetna.Models.hairdresserEntity", null)
                        .WithMany("haircuts")
                        .HasForeignKey("hairdresserEntityid_hairdresser");
                });

            modelBuilder.Entity("maturitetna.Models.hairdresserHaircutEntity", b =>
                {
                    b.HasOne("maturitetna.Models.haircutEntity", "haircut")
                        .WithMany("hairdressers")
                        .HasForeignKey("id_haircut")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("maturitetna.Models.hairdresserEntity", "hairdresser")
                        .WithMany()
                        .HasForeignKey("id_hairdresser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("haircut");

                    b.Navigation("hairdresser");
                });

            modelBuilder.Entity("maturitetna.Models.haircutEntity", b =>
                {
                    b.Navigation("hairdressers");
                });

            modelBuilder.Entity("maturitetna.Models.hairdresserEntity", b =>
                {
                    b.Navigation("appointements");

                    b.Navigation("haircuts");
                });
#pragma warning restore 612, 618
        }
    }
}
