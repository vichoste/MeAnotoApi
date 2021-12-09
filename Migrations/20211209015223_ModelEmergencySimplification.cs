using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeAnotoApi.Migrations
{
    public partial class ModelEmergencySimplification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventInstances_CourseInstances_CourseInstanceId",
                table: "EventInstances");

            migrationBuilder.DropForeignKey(
                name: "FK_EventInstances_Rooms_RoomId",
                table: "EventInstances");

            migrationBuilder.DropTable(
                name: "AttendeeCourseInstance");

            migrationBuilder.DropTable(
                name: "CourseInstanceProfessor");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "CourseInstances");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Careers");

            migrationBuilder.DropTable(
                name: "CampusSingulars");

            migrationBuilder.DropIndex(
                name: "IX_EventInstances_CourseInstanceId",
                table: "EventInstances");

            migrationBuilder.DropIndex(
                name: "IX_EventInstances_RoomId",
                table: "EventInstances");

            migrationBuilder.DropColumn(
                name: "CourseInstanceId",
                table: "EventInstances");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "EventInstances");

            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Events");

            migrationBuilder.AddColumn<int>(
                name: "CourseInstanceId",
                table: "EventInstances",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "EventInstances",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CampusSingulars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    InstitutionId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampusSingulars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampusSingulars_Institutions_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "Institutions",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Careers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CampusSingularId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Careers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Careers_CampusSingulars_CampusSingularId",
                        column: x => x.CampusSingularId,
                        principalTable: "CampusSingulars",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CampusSingularId = table.Column<int>(type: "int", nullable: true),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_CampusSingulars_CampusSingularId",
                        column: x => x.CampusSingularId,
                        principalTable: "CampusSingulars",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CareerId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Careers_CareerId",
                        column: x => x.CareerId,
                        principalTable: "Careers",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CourseInstances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CourseId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Section = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Semester = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Year = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseInstances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseInstances_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AttendeeCourseInstance",
                columns: table => new
                {
                    AttendeesId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CourseInstancesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
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
                columns: table => new
                {
                    CourseInstancesId = table.Column<int>(type: "int", nullable: false),
                    ProfessorsId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
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

            migrationBuilder.CreateIndex(
                name: "IX_EventInstances_CourseInstanceId",
                table: "EventInstances",
                column: "CourseInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_EventInstances_RoomId",
                table: "EventInstances",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendeeCourseInstance_CourseInstancesId",
                table: "AttendeeCourseInstance",
                column: "CourseInstancesId");

            migrationBuilder.CreateIndex(
                name: "IX_CampusSingulars_InstitutionId",
                table: "CampusSingulars",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_Careers_CampusSingularId",
                table: "Careers",
                column: "CampusSingularId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseInstanceProfessor_ProfessorsId",
                table: "CourseInstanceProfessor",
                column: "ProfessorsId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseInstances_CourseId",
                table: "CourseInstances",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CareerId",
                table: "Courses",
                column: "CareerId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_CampusSingularId",
                table: "Rooms",
                column: "CampusSingularId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventInstances_CourseInstances_CourseInstanceId",
                table: "EventInstances",
                column: "CourseInstanceId",
                principalTable: "CourseInstances",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EventInstances_Rooms_RoomId",
                table: "EventInstances",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");
        }
    }
}
