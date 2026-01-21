using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Migrations
{
    /// <inheritdoc />
    public partial class icon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Badges",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Badges",
                keyColumn: "BadgeId",
                keyValue: 1,
                column: "Icon",
                value: "bi-award-fill");

            migrationBuilder.UpdateData(
                table: "Badges",
                keyColumn: "BadgeId",
                keyValue: 2,
                column: "Icon",
                value: "bi-award-fill");

            migrationBuilder.UpdateData(
                table: "Badges",
                keyColumn: "BadgeId",
                keyValue: 3,
                column: "Icon",
                value: "bi-award-fill");

            migrationBuilder.UpdateData(
                table: "Badges",
                keyColumn: "BadgeId",
                keyValue: 4,
                column: "Icon",
                value: "bi-award-fill");

            migrationBuilder.UpdateData(
                table: "Badges",
                keyColumn: "BadgeId",
                keyValue: 5,
                column: "Icon",
                value: "bi-award-fill");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Badges");
        }
    }
}
