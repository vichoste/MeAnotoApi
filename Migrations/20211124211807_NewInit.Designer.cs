﻿// <auto-generated />
using System;
using MeAnotoApi.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MeAnotoApi.Migrations
{
    [DbContext(typeof(MeAnotoContext))]
    [Migration("20211124211807_NewInit")]
    partial class NewInit
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("AttendeeCourseInstance", b =>
                {
                    b.Property<string>("AttendeesId")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("CourseInstancesId")
                        .HasColumnType("int");

                    b.HasKey("AttendeesId", "CourseInstancesId");

                    b.HasIndex("CourseInstancesId");

                    b.ToTable("AttendeeCourseInstance");
                });

            modelBuilder.Entity("AttendeeEventInstance", b =>
                {
                    b.Property<string>("AttendeesId")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("EventInstancesId")
                        .HasColumnType("int");

                    b.HasKey("AttendeesId", "EventInstancesId");

                    b.HasIndex("EventInstancesId");

                    b.ToTable("AttendeeEventInstance");
                });

            modelBuilder.Entity("CourseInstanceProfessor", b =>
                {
                    b.Property<int>("CourseInstancesId")
                        .HasColumnType("int");

                    b.Property<string>("ProfessorsId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("CourseInstancesId", "ProfessorsId");

                    b.HasIndex("ProfessorsId");

                    b.ToTable("CourseInstanceProfessor");
                });

            modelBuilder.Entity("MeAnotoApi.Models.Entities.CampusSingular", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("InstitutionId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("InstitutionId");

                    b.ToTable("CampusSingulars");
                });

            modelBuilder.Entity("MeAnotoApi.Models.Entities.Career", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("CampusSingularId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("CampusSingularId");

                    b.ToTable("Careers");
                });

            modelBuilder.Entity("MeAnotoApi.Models.Entities.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("CareerId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("CareerId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("MeAnotoApi.Models.Entities.CourseInstance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("CourseId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("Section")
                        .HasColumnType("longtext");

                    b.Property<string>("Semester")
                        .HasColumnType("longtext");

                    b.Property<string>("Year")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("CourseInstances");
                });

            modelBuilder.Entity("MeAnotoApi.Models.Entities.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("InstitutionId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("ProfessorId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("InstitutionId");

                    b.HasIndex("ProfessorId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("MeAnotoApi.Models.Entities.EventInstance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Cancellation")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("CourseInstanceId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Creation")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("EventId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<int?>("RoomId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Schedule")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("CourseInstanceId");

                    b.HasIndex("EventId");

                    b.HasIndex("RoomId");

                    b.ToTable("EventInstances");
                });

            modelBuilder.Entity("MeAnotoApi.Models.Entities.Institution", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Institutions");
                });

            modelBuilder.Entity("MeAnotoApi.Models.Entities.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("CampusSingularId")
                        .HasColumnType("int");

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("CampusSingularId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("MeAnotoApi.Models.Users.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("FirstName")
                        .HasColumnType("longtext");

                    b.Property<int?>("InstitutionId")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .HasColumnType("longtext");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Run")
                        .HasColumnType("longtext");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("InstitutionId");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("ApplicationUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("MeAnotoApi.Models.Users.Attendee", b =>
                {
                    b.HasBaseType("MeAnotoApi.Models.Users.ApplicationUser");

                    b.HasDiscriminator().HasValue("Attendee");
                });

            modelBuilder.Entity("MeAnotoApi.Models.Users.Professor", b =>
                {
                    b.HasBaseType("MeAnotoApi.Models.Users.ApplicationUser");

                    b.HasDiscriminator().HasValue("Professor");
                });

            modelBuilder.Entity("AttendeeCourseInstance", b =>
                {
                    b.HasOne("MeAnotoApi.Models.Users.Attendee", null)
                        .WithMany()
                        .HasForeignKey("AttendeesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MeAnotoApi.Models.Entities.CourseInstance", null)
                        .WithMany()
                        .HasForeignKey("CourseInstancesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AttendeeEventInstance", b =>
                {
                    b.HasOne("MeAnotoApi.Models.Users.Attendee", null)
                        .WithMany()
                        .HasForeignKey("AttendeesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MeAnotoApi.Models.Entities.EventInstance", null)
                        .WithMany()
                        .HasForeignKey("EventInstancesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CourseInstanceProfessor", b =>
                {
                    b.HasOne("MeAnotoApi.Models.Entities.CourseInstance", null)
                        .WithMany()
                        .HasForeignKey("CourseInstancesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MeAnotoApi.Models.Users.Professor", null)
                        .WithMany()
                        .HasForeignKey("ProfessorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MeAnotoApi.Models.Entities.CampusSingular", b =>
                {
                    b.HasOne("MeAnotoApi.Models.Entities.Institution", "Institution")
                        .WithMany("CampusSingulars")
                        .HasForeignKey("InstitutionId");

                    b.Navigation("Institution");
                });

            modelBuilder.Entity("MeAnotoApi.Models.Entities.Career", b =>
                {
                    b.HasOne("MeAnotoApi.Models.Entities.CampusSingular", "CampusSingular")
                        .WithMany("Careers")
                        .HasForeignKey("CampusSingularId");

                    b.Navigation("CampusSingular");
                });

            modelBuilder.Entity("MeAnotoApi.Models.Entities.Course", b =>
                {
                    b.HasOne("MeAnotoApi.Models.Entities.Career", "Career")
                        .WithMany("Courses")
                        .HasForeignKey("CareerId");

                    b.Navigation("Career");
                });

            modelBuilder.Entity("MeAnotoApi.Models.Entities.CourseInstance", b =>
                {
                    b.HasOne("MeAnotoApi.Models.Entities.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId");

                    b.Navigation("Course");
                });

            modelBuilder.Entity("MeAnotoApi.Models.Entities.Event", b =>
                {
                    b.HasOne("MeAnotoApi.Models.Entities.Institution", "Institution")
                        .WithMany("Events")
                        .HasForeignKey("InstitutionId");

                    b.HasOne("MeAnotoApi.Models.Users.Professor", "Professor")
                        .WithMany("Events")
                        .HasForeignKey("ProfessorId");

                    b.Navigation("Institution");

                    b.Navigation("Professor");
                });

            modelBuilder.Entity("MeAnotoApi.Models.Entities.EventInstance", b =>
                {
                    b.HasOne("MeAnotoApi.Models.Entities.CourseInstance", "CourseInstance")
                        .WithMany("EventInstances")
                        .HasForeignKey("CourseInstanceId");

                    b.HasOne("MeAnotoApi.Models.Entities.Event", "Event")
                        .WithMany("EventInstances")
                        .HasForeignKey("EventId");

                    b.HasOne("MeAnotoApi.Models.Entities.Room", "Room")
                        .WithMany("EventInstances")
                        .HasForeignKey("RoomId");

                    b.Navigation("CourseInstance");

                    b.Navigation("Event");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("MeAnotoApi.Models.Entities.Room", b =>
                {
                    b.HasOne("MeAnotoApi.Models.Entities.CampusSingular", "CampusSingular")
                        .WithMany("Rooms")
                        .HasForeignKey("CampusSingularId");

                    b.Navigation("CampusSingular");
                });

            modelBuilder.Entity("MeAnotoApi.Models.Users.ApplicationUser", b =>
                {
                    b.HasOne("MeAnotoApi.Models.Entities.Institution", "Institution")
                        .WithMany()
                        .HasForeignKey("InstitutionId");

                    b.Navigation("Institution");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("MeAnotoApi.Models.Users.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("MeAnotoApi.Models.Users.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MeAnotoApi.Models.Users.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("MeAnotoApi.Models.Users.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MeAnotoApi.Models.Entities.CampusSingular", b =>
                {
                    b.Navigation("Careers");

                    b.Navigation("Rooms");
                });

            modelBuilder.Entity("MeAnotoApi.Models.Entities.Career", b =>
                {
                    b.Navigation("Courses");
                });

            modelBuilder.Entity("MeAnotoApi.Models.Entities.CourseInstance", b =>
                {
                    b.Navigation("EventInstances");
                });

            modelBuilder.Entity("MeAnotoApi.Models.Entities.Event", b =>
                {
                    b.Navigation("EventInstances");
                });

            modelBuilder.Entity("MeAnotoApi.Models.Entities.Institution", b =>
                {
                    b.Navigation("CampusSingulars");

                    b.Navigation("Events");
                });

            modelBuilder.Entity("MeAnotoApi.Models.Entities.Room", b =>
                {
                    b.Navigation("EventInstances");
                });

            modelBuilder.Entity("MeAnotoApi.Models.Users.Professor", b =>
                {
                    b.Navigation("Events");
                });
#pragma warning restore 612, 618
        }
    }
}
