using System;

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MeAnotoApi.Migrations {
	public partial class ModelComplete : Migration {
		protected override void Up(MigrationBuilder migrationBuilder) {
			migrationBuilder.CreateTable(
				name: "CourseInstances",
				columns: table => new {
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
					CourseId = table.Column<int>(type: "int", nullable: true),
					Year = table.Column<string>(type: "longtext", nullable: true)
						.Annotation("MySql:CharSet", "utf8mb4"),
					Semester = table.Column<string>(type: "longtext", nullable: true)
						.Annotation("MySql:CharSet", "utf8mb4"),
					Section = table.Column<string>(type: "longtext", nullable: true)
						.Annotation("MySql:CharSet", "utf8mb4"),
					Name = table.Column<string>(type: "longtext", nullable: true)
						.Annotation("MySql:CharSet", "utf8mb4")
				},
				constraints: table => {
					table.PrimaryKey("PK_CourseInstances", x => x.Id);
					table.ForeignKey(
						name: "FK_CourseInstances_Courses_CourseId",
						column: x => x.CourseId,
						principalTable: "Courses",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				})
				.Annotation("MySql:CharSet", "utf8mb4");

			migrationBuilder.CreateTable(
				name: "Events",
				columns: table => new {
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
					ProfessorId = table.Column<string>(type: "varchar(255)", nullable: true)
						.Annotation("MySql:CharSet", "utf8mb4"),
					InstitutionId = table.Column<int>(type: "int", nullable: true),
					Name = table.Column<string>(type: "longtext", nullable: true)
						.Annotation("MySql:CharSet", "utf8mb4")
				},
				constraints: table => {
					table.PrimaryKey("PK_Events", x => x.Id);
					table.ForeignKey(
						name: "FK_Events_AspNetUsers_ProfessorId",
						column: x => x.ProfessorId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Events_Institutions_InstitutionId",
						column: x => x.InstitutionId,
						principalTable: "Institutions",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				})
				.Annotation("MySql:CharSet", "utf8mb4");

			migrationBuilder.CreateTable(
				name: "Rooms",
				columns: table => new {
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
					Capacity = table.Column<int>(type: "int", nullable: false),
					CampusSingularId = table.Column<int>(type: "int", nullable: true),
					Name = table.Column<string>(type: "longtext", nullable: true)
						.Annotation("MySql:CharSet", "utf8mb4")
				},
				constraints: table => {
					table.PrimaryKey("PK_Rooms", x => x.Id);
					table.ForeignKey(
						name: "FK_Rooms_CampusSingulars_CampusSingularId",
						column: x => x.CampusSingularId,
						principalTable: "CampusSingulars",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				})
				.Annotation("MySql:CharSet", "utf8mb4");

			migrationBuilder.CreateTable(
				name: "AttendeeCourseInstance",
				columns: table => new {
					AttendeesId = table.Column<string>(type: "varchar(255)", nullable: false)
						.Annotation("MySql:CharSet", "utf8mb4"),
					CourseInstancesId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table => {
					table.PrimaryKey("PK_AttendeeCourseInstance", x => new { x.AttendeesId, x.CourseInstancesId });
					table.ForeignKey(
						name: "FK_AttendeeCourseInstance_AspNetUsers_AttendeesId",
						column: x => x.AttendeesId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_AttendeeCourseInstance_CourseInstances_CourseInstancesId",
						column: x => x.CourseInstancesId,
						principalTable: "CourseInstances",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				})
				.Annotation("MySql:CharSet", "utf8mb4");

			migrationBuilder.CreateTable(
				name: "CourseInstanceProfessor",
				columns: table => new {
					CourseInstancesId = table.Column<int>(type: "int", nullable: false),
					ProfessorsId = table.Column<string>(type: "varchar(255)", nullable: false)
						.Annotation("MySql:CharSet", "utf8mb4")
				},
				constraints: table => {
					table.PrimaryKey("PK_CourseInstanceProfessor", x => new { x.CourseInstancesId, x.ProfessorsId });
					table.ForeignKey(
						name: "FK_CourseInstanceProfessor_AspNetUsers_ProfessorsId",
						column: x => x.ProfessorsId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_CourseInstanceProfessor_CourseInstances_CourseInstancesId",
						column: x => x.CourseInstancesId,
						principalTable: "CourseInstances",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				})
				.Annotation("MySql:CharSet", "utf8mb4");

			migrationBuilder.CreateTable(
				name: "EventInstances",
				columns: table => new {
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
					EventId = table.Column<int>(type: "int", nullable: true),
					CourseInstanceId = table.Column<int>(type: "int", nullable: true),
					RoomId = table.Column<int>(type: "int", nullable: true),
					DateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
					Creation = table.Column<DateTime>(type: "datetime(6)", nullable: false),
					Cancellation = table.Column<DateTime>(type: "datetime(6)", nullable: false),
					Name = table.Column<string>(type: "longtext", nullable: true)
						.Annotation("MySql:CharSet", "utf8mb4")
				},
				constraints: table => {
					table.PrimaryKey("PK_EventInstances", x => x.Id);
					table.ForeignKey(
						name: "FK_EventInstances_CourseInstances_CourseInstanceId",
						column: x => x.CourseInstanceId,
						principalTable: "CourseInstances",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_EventInstances_Events_EventId",
						column: x => x.EventId,
						principalTable: "Events",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_EventInstances_Rooms_RoomId",
						column: x => x.RoomId,
						principalTable: "Rooms",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				})
				.Annotation("MySql:CharSet", "utf8mb4");

			migrationBuilder.CreateTable(
				name: "AttendeeEventInstance",
				columns: table => new {
					AttendeesId = table.Column<string>(type: "varchar(255)", nullable: false)
						.Annotation("MySql:CharSet", "utf8mb4"),
					EventInstancesId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table => {
					table.PrimaryKey("PK_AttendeeEventInstance", x => new { x.AttendeesId, x.EventInstancesId });
					table.ForeignKey(
						name: "FK_AttendeeEventInstance_AspNetUsers_AttendeesId",
						column: x => x.AttendeesId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_AttendeeEventInstance_EventInstances_EventInstancesId",
						column: x => x.EventInstancesId,
						principalTable: "EventInstances",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				})
				.Annotation("MySql:CharSet", "utf8mb4");

			migrationBuilder.CreateIndex(
				name: "IX_AttendeeCourseInstance_CourseInstancesId",
				table: "AttendeeCourseInstance",
				column: "CourseInstancesId");

			migrationBuilder.CreateIndex(
				name: "IX_AttendeeEventInstance_EventInstancesId",
				table: "AttendeeEventInstance",
				column: "EventInstancesId");

			migrationBuilder.CreateIndex(
				name: "IX_CourseInstanceProfessor_ProfessorsId",
				table: "CourseInstanceProfessor",
				column: "ProfessorsId");

			migrationBuilder.CreateIndex(
				name: "IX_CourseInstances_CourseId",
				table: "CourseInstances",
				column: "CourseId");

			migrationBuilder.CreateIndex(
				name: "IX_EventInstances_CourseInstanceId",
				table: "EventInstances",
				column: "CourseInstanceId");

			migrationBuilder.CreateIndex(
				name: "IX_EventInstances_EventId",
				table: "EventInstances",
				column: "EventId");

			migrationBuilder.CreateIndex(
				name: "IX_EventInstances_RoomId",
				table: "EventInstances",
				column: "RoomId");

			migrationBuilder.CreateIndex(
				name: "IX_Events_InstitutionId",
				table: "Events",
				column: "InstitutionId");

			migrationBuilder.CreateIndex(
				name: "IX_Events_ProfessorId",
				table: "Events",
				column: "ProfessorId");

			migrationBuilder.CreateIndex(
				name: "IX_Rooms_CampusSingularId",
				table: "Rooms",
				column: "CampusSingularId");
		}

		protected override void Down(MigrationBuilder migrationBuilder) {
			migrationBuilder.DropTable(
				name: "AttendeeCourseInstance");

			migrationBuilder.DropTable(
				name: "AttendeeEventInstance");

			migrationBuilder.DropTable(
				name: "CourseInstanceProfessor");

			migrationBuilder.DropTable(
				name: "EventInstances");

			migrationBuilder.DropTable(
				name: "CourseInstances");

			migrationBuilder.DropTable(
				name: "Events");

			migrationBuilder.DropTable(
				name: "Rooms");
		}
	}
}
