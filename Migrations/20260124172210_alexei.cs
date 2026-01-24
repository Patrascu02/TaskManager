using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Migrations
{
    /// <inheritdoc />
    public partial class alexei : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "UserTasks",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ManagerDifficulty",
                table: "UserTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserTasks_CreatedById",
                table: "UserTasks",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTasks_AspNetUsers_CreatedById",
                table: "UserTasks",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTasks_AspNetUsers_CreatedById",
                table: "UserTasks");

            migrationBuilder.DropIndex(
                name: "IX_UserTasks_CreatedById",
                table: "UserTasks");

            migrationBuilder.DropColumn(
                name: "ManagerDifficulty",
                table: "UserTasks");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "UserTasks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
