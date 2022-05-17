using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DigitalSignEditor.Migrations
{
    public partial class AddMaximumSize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaximumSize",
                table: "Signs",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaximumSize",
                table: "Signs");

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
    }
}
