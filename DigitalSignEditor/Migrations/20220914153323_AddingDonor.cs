using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DigitalSignEditor.Migrations
{
    public partial class AddingDonor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Donors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PersonName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donors", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Signs",
                keyColumn: "Id",
                keyValue: -4,
                column: "LastUpdated",
                value: new DateTime(2022, 9, 14, 10, 33, 22, 826, DateTimeKind.Local).AddTicks(4142));

            migrationBuilder.UpdateData(
                table: "Signs",
                keyColumn: "Id",
                keyValue: -3,
                column: "LastUpdated",
                value: new DateTime(2022, 9, 14, 10, 33, 22, 826, DateTimeKind.Local).AddTicks(4123));

            migrationBuilder.UpdateData(
                table: "Signs",
                keyColumn: "Id",
                keyValue: -2,
                column: "LastUpdated",
                value: new DateTime(2022, 9, 14, 10, 33, 22, 826, DateTimeKind.Local).AddTicks(3757));

            migrationBuilder.UpdateData(
                table: "Signs",
                keyColumn: "Id",
                keyValue: -1,
                column: "LastUpdated",
                value: new DateTime(2022, 9, 14, 10, 33, 22, 815, DateTimeKind.Local).AddTicks(2401));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Donors");

            migrationBuilder.UpdateData(
                table: "Signs",
                keyColumn: "Id",
                keyValue: -4,
                column: "LastUpdated",
                value: new DateTime(2022, 5, 13, 17, 7, 43, 322, DateTimeKind.Local).AddTicks(8328));

            migrationBuilder.UpdateData(
                table: "Signs",
                keyColumn: "Id",
                keyValue: -3,
                column: "LastUpdated",
                value: new DateTime(2022, 5, 13, 17, 7, 43, 322, DateTimeKind.Local).AddTicks(8321));

            migrationBuilder.UpdateData(
                table: "Signs",
                keyColumn: "Id",
                keyValue: -2,
                column: "LastUpdated",
                value: new DateTime(2022, 5, 13, 17, 7, 43, 322, DateTimeKind.Local).AddTicks(8196));

            migrationBuilder.UpdateData(
                table: "Signs",
                keyColumn: "Id",
                keyValue: -1,
                column: "LastUpdated",
                value: new DateTime(2022, 5, 13, 17, 7, 43, 319, DateTimeKind.Local).AddTicks(5021));
        }
    }
}
