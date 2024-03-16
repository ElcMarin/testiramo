using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace maturitetna.Migrations
{
    /// <inheritdoc />
    public partial class MigrationsForRescheduling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "created",
                table: "appointment",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "rescheduleIn14Days",
                table: "appointment",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "reschedulingId",
                table: "appointment",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created",
                table: "appointment");

            migrationBuilder.DropColumn(
                name: "rescheduleIn14Days",
                table: "appointment");

            migrationBuilder.DropColumn(
                name: "reschedulingId",
                table: "appointment");
        }
    }
}
