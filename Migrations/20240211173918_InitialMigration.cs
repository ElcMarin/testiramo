using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace maturitetna.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "admin",
                columns: table => new
                {
                    id_admin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    lastname = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admin", x => x.id_admin);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "hairdresser",
                columns: table => new
                {
                    id_hairdresser = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    lastname = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    gender = table.Column<int>(type: "int", nullable: true),
                    is_working = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    startTime = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    endTime = table.Column<TimeSpan>(type: "time(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hairdresser", x => x.id_hairdresser);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id_user = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    lastname = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.id_user);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "haircut",
                columns: table => new
                {
                    id_haircut = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    style = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    image = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    duration = table.Column<int>(type: "int", nullable: false),
                    hairdresserEntityid_hairdresser = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_haircut", x => x.id_haircut);
                    table.ForeignKey(
                        name: "FK_haircut_hairdresser_hairdresserEntityid_hairdresser",
                        column: x => x.hairdresserEntityid_hairdresser,
                        principalTable: "hairdresser",
                        principalColumn: "id_hairdresser");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "appointment",
                columns: table => new
                {
                    id_appointment = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_user = table.Column<int>(type: "int", nullable: false),
                    id_hairdresser = table.Column<int>(type: "int", nullable: false),
                    id_haircut = table.Column<int>(type: "int", nullable: false),
                    appointmentTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appointment", x => x.id_appointment);
                    table.ForeignKey(
                        name: "FK_appointment_haircut_id_haircut",
                        column: x => x.id_haircut,
                        principalTable: "haircut",
                        principalColumn: "id_haircut",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_appointment_hairdresser_id_hairdresser",
                        column: x => x.id_hairdresser,
                        principalTable: "hairdresser",
                        principalColumn: "id_hairdresser",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_appointment_user_id_user",
                        column: x => x.id_user,
                        principalTable: "user",
                        principalColumn: "id_user",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "hairdresserHaircut",
                columns: table => new
                {
                    id_hairdresser = table.Column<int>(type: "int", nullable: false),
                    id_haircut = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hairdresserHaircut", x => new { x.id_hairdresser, x.id_haircut });
                    table.ForeignKey(
                        name: "FK_hairdresserHaircut_haircut_id_haircut",
                        column: x => x.id_haircut,
                        principalTable: "haircut",
                        principalColumn: "id_haircut",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_hairdresserHaircut_hairdresser_id_hairdresser",
                        column: x => x.id_hairdresser,
                        principalTable: "hairdresser",
                        principalColumn: "id_hairdresser",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_appointment_id_haircut",
                table: "appointment",
                column: "id_haircut");

            migrationBuilder.CreateIndex(
                name: "IX_appointment_id_hairdresser",
                table: "appointment",
                column: "id_hairdresser");

            migrationBuilder.CreateIndex(
                name: "IX_appointment_id_user",
                table: "appointment",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "IX_haircut_hairdresserEntityid_hairdresser",
                table: "haircut",
                column: "hairdresserEntityid_hairdresser");

            migrationBuilder.CreateIndex(
                name: "IX_hairdresserHaircut_id_haircut",
                table: "hairdresserHaircut",
                column: "id_haircut");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admin");

            migrationBuilder.DropTable(
                name: "appointment");

            migrationBuilder.DropTable(
                name: "hairdresserHaircut");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "haircut");

            migrationBuilder.DropTable(
                name: "hairdresser");
        }
    }
}
