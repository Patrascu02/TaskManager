using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Migrations
{
    /// <inheritdoc />
    public partial class alexx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentBadgeId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CurrentBadgeId",
                table: "AspNetUsers",
                column: "CurrentBadgeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Badges_CurrentBadgeId",
                table: "AspNetUsers",
                column: "CurrentBadgeId",
                principalTable: "Badges",
                principalColumn: "BadgeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Badges_CurrentBadgeId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CurrentBadgeId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CurrentBadgeId",
                table: "AspNetUsers");
        }
    }
}
