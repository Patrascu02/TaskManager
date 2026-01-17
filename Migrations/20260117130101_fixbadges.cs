using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskManager.Migrations
{
    /// <inheritdoc />
    public partial class fixbadges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Badges",
                columns: new[] { "BadgeId", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Ai finalizat primul tău task!", "Novice" },
                    { 2, "Ai finalizat 5 task-uri.", "Harnic" },
                    { 3, "Ai atins nivelul 5.", "Expert" },
                    { 4, "Ai finalizat 20 de task-uri.", "Veteran" },
                    { 5, "Ai terminat 3 task-uri înainte de termen.", "Punctual" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "BadgeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "BadgeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "BadgeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "BadgeId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Badges",
                keyColumn: "BadgeId",
                keyValue: 5);
        }
    }
}
