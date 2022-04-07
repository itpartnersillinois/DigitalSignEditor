using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DigitalSignEditor.Migrations
{
    public partial class AddChangeIndicator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChangeIndicators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeIndicators", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Signs",
                keyColumn: "Id",
                keyValue: -4,
                column: "LastUpdated",
                value: new DateTime(2022, 4, 7, 15, 7, 52, 226, DateTimeKind.Local).AddTicks(9548));

            migrationBuilder.UpdateData(
                table: "Signs",
                keyColumn: "Id",
                keyValue: -3,
                column: "LastUpdated",
                value: new DateTime(2022, 4, 7, 15, 7, 52, 226, DateTimeKind.Local).AddTicks(9542));

            migrationBuilder.UpdateData(
                table: "Signs",
                keyColumn: "Id",
                keyValue: -2,
                column: "LastUpdated",
                value: new DateTime(2022, 4, 7, 15, 7, 52, 226, DateTimeKind.Local).AddTicks(9451));

            migrationBuilder.UpdateData(
                table: "Signs",
                keyColumn: "Id",
                keyValue: -1,
                column: "LastUpdated",
                value: new DateTime(2022, 4, 7, 15, 7, 52, 223, DateTimeKind.Local).AddTicks(6549));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChangeIndicators");

            migrationBuilder.UpdateData(
                table: "Signs",
                keyColumn: "Id",
                keyValue: -4,
                column: "LastUpdated",
                value: new DateTime(2022, 4, 1, 14, 55, 36, 893, DateTimeKind.Local).AddTicks(5409));

            migrationBuilder.UpdateData(
                table: "Signs",
                keyColumn: "Id",
                keyValue: -3,
                column: "LastUpdated",
                value: new DateTime(2022, 4, 1, 14, 55, 36, 893, DateTimeKind.Local).AddTicks(5404));

            migrationBuilder.UpdateData(
                table: "Signs",
                keyColumn: "Id",
                keyValue: -2,
                column: "LastUpdated",
                value: new DateTime(2022, 4, 1, 14, 55, 36, 893, DateTimeKind.Local).AddTicks(5314));

            migrationBuilder.UpdateData(
                table: "Signs",
                keyColumn: "Id",
                keyValue: -1,
                column: "LastUpdated",
                value: new DateTime(2022, 4, 1, 14, 55, 36, 890, DateTimeKind.Local).AddTicks(3281));
        }
    }
}
