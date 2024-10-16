﻿// <auto-generated />
using System;
using BlazorDemoApp.InfraSructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BlazorDemoApp.InfraSructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240717115350_m0")]
    partial class m0
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BlazorDemoApp.Shared.Classes.TableClass.Add0_Gov", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int?>("Modify_by")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Modify_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("NameAr")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("NameEn")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<int?>("added_by")
                        .HasColumnType("int");

                    b.Property<DateTime?>("added_date")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("added_by");

                    b.ToTable("Add0Govs");
                });

            modelBuilder.Entity("BlazorDemoApp.Shared.Classes.TableClass.Add1_Markaz", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("IdFK_Gov")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int?>("Modify_by")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Modify_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("NameAr")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("NameEn")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<int?>("added_by")
                        .HasColumnType("int");

                    b.Property<DateTime?>("added_date")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("IdFK_Gov");

                    b.HasIndex("added_by");

                    b.ToTable("Add1Markazs");
                });

            modelBuilder.Entity("BlazorDemoApp.Shared.Classes.TableClass.Add2_City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("IdFK_Markaz")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int?>("Modify_by")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Modify_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("NameAr")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("NameEn")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<int?>("added_by")
                        .HasColumnType("int");

                    b.Property<DateTime?>("added_date")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("IdFK_Markaz");

                    b.HasIndex("added_by");

                    b.ToTable("Add2Cities");
                });

            modelBuilder.Entity("BlazorDemoApp.Shared.Classes.TableClass.MyAppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Ans1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ans2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("IDFK_MyAppUser")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int?>("Modify_by")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Modify_date")
                        .HasColumnType("datetime2");

                    b.Property<short?>("Q1")
                        .HasColumnType("smallint");

                    b.Property<short?>("Q2")
                        .HasColumnType("smallint");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserPass")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("added_by")
                        .HasColumnType("int");

                    b.Property<DateTime?>("added_date")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("isEmp")
                        .HasColumnType("bit");

                    b.Property<bool?>("isParent")
                        .HasColumnType("bit");

                    b.Property<bool?>("isPasswordReset")
                        .HasColumnType("bit");

                    b.Property<bool?>("isStd")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("IDFK_MyAppUser");

                    b.ToTable("MyAppUsers");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Ans1 = "admin1",
                            Ans2 = "admin2",
                            IDFK_MyAppUser = 1,
                            IsActive = true,
                            Q1 = (short)1,
                            Q2 = (short)2,
                            UserName = "admin",
                            UserPass = "admin@12345",
                            isEmp = true,
                            isPasswordReset = false
                        },
                        new
                        {
                            Id = 2,
                            Ans1 = "emp1emp1",
                            Ans2 = "emp11emp11",
                            IDFK_MyAppUser = 2,
                            IsActive = true,
                            Q1 = (short)1,
                            Q2 = (short)2,
                            UserName = "emp1",
                            UserPass = "emp1@12345",
                            isEmp = true,
                            isPasswordReset = true
                        },
                        new
                        {
                            Id = 3,
                            Ans1 = "emp2emp2",
                            Ans2 = "emp22emp22",
                            IDFK_MyAppUser = 3,
                            IsActive = true,
                            Q1 = (short)1,
                            Q2 = (short)2,
                            UserName = "emp2",
                            UserPass = "emp2@12345",
                            isEmp = true,
                            isPasswordReset = false
                        },
                        new
                        {
                            Id = 4,
                            Ans1 = "parent",
                            Ans2 = "parent1",
                            IDFK_MyAppUser = 4,
                            IsActive = true,
                            Q1 = (short)1,
                            Q2 = (short)2,
                            UserName = "Parent",
                            UserPass = "parent@12345",
                            isEmp = false,
                            isParent = true,
                            isPasswordReset = false
                        });
                });

            modelBuilder.Entity("BlazorDemoApp.Shared.Classes.TableClass.Add0_Gov", b =>
                {
                    b.HasOne("BlazorDemoApp.Shared.Classes.TableClass.MyAppUser", "MyAppUser")
                        .WithMany("Add0_Gov")
                        .HasForeignKey("added_by");

                    b.Navigation("MyAppUser");
                });

            modelBuilder.Entity("BlazorDemoApp.Shared.Classes.TableClass.Add1_Markaz", b =>
                {
                    b.HasOne("BlazorDemoApp.Shared.Classes.TableClass.Add0_Gov", "Add0_Gov")
                        .WithMany("Markazs")
                        .HasForeignKey("IdFK_Gov")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlazorDemoApp.Shared.Classes.TableClass.MyAppUser", "MyAppUser")
                        .WithMany("Add1_Markaz")
                        .HasForeignKey("added_by");

                    b.Navigation("Add0_Gov");

                    b.Navigation("MyAppUser");
                });

            modelBuilder.Entity("BlazorDemoApp.Shared.Classes.TableClass.Add2_City", b =>
                {
                    b.HasOne("BlazorDemoApp.Shared.Classes.TableClass.Add1_Markaz", "Add1_Markaz")
                        .WithMany("Add2_City")
                        .HasForeignKey("IdFK_Markaz")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlazorDemoApp.Shared.Classes.TableClass.MyAppUser", "MyAppUser")
                        .WithMany("Add2_City")
                        .HasForeignKey("added_by");

                    b.Navigation("Add1_Markaz");

                    b.Navigation("MyAppUser");
                });

            modelBuilder.Entity("BlazorDemoApp.Shared.Classes.TableClass.MyAppUser", b =>
                {
                    b.HasOne("BlazorDemoApp.Shared.Classes.TableClass.MyAppUser", "Reg_by")
                        .WithMany("LMyAppUser")
                        .HasForeignKey("IDFK_MyAppUser");

                    b.Navigation("Reg_by");
                });

            modelBuilder.Entity("BlazorDemoApp.Shared.Classes.TableClass.Add0_Gov", b =>
                {
                    b.Navigation("Markazs");
                });

            modelBuilder.Entity("BlazorDemoApp.Shared.Classes.TableClass.Add1_Markaz", b =>
                {
                    b.Navigation("Add2_City");
                });

            modelBuilder.Entity("BlazorDemoApp.Shared.Classes.TableClass.MyAppUser", b =>
                {
                    b.Navigation("Add0_Gov");

                    b.Navigation("Add1_Markaz");

                    b.Navigation("Add2_City");

                    b.Navigation("LMyAppUser");
                });
#pragma warning restore 612, 618
        }
    }
}
