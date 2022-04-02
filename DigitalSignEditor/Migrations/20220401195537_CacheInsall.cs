using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DigitalSignEditor.Migrations
{
    public partial class CacheInsall : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CacheItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinutesUntilCacheExpires = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CacheItems", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CacheItems");

            migrationBuilder.UpdateData(
                table: "Signs",
                keyColumn: "Id",
                keyValue: -4,
                column: "LastUpdated",
                value: new DateTime(2022, 3, 18, 9, 56, 19, 63, DateTimeKind.Local).AddTicks(1045));

            migrationBuilder.UpdateData(
                table: "Signs",
                keyColumn: "Id",
                keyValue: -3,
                column: "LastUpdated",
                value: new DateTime(2022, 3, 18, 9, 56, 19, 63, DateTimeKind.Local).AddTicks(1040));

            migrationBuilder.UpdateData(
                table: "Signs",
                keyColumn: "Id",
                keyValue: -2,
                column: "LastUpdated",
                value: new DateTime(2022, 3, 18, 9, 56, 19, 63, DateTimeKind.Local).AddTicks(953));

            migrationBuilder.UpdateData(
                table: "Signs",
                keyColumn: "Id",
                keyValue: -1,
                column: "LastUpdated",
                value: new DateTime(2022, 3, 18, 9, 56, 19, 59, DateTimeKind.Local).AddTicks(8283));
        }
    }
}
