using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Migrations
{
    /// <inheritdoc />
    public partial class icon2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // --- ȘTERGE SAU COMENTEAZĂ ACESTE LINII ---
            // migrationBuilder.AddColumn<string>(
            //    name: "Icon",
            //    table: "Badges",
            //    type: "nvarchar(max)",
            //    nullable: false,
            //    defaultValue: "bi-award-fill");
            // ------------------------------------------

            // --- PĂSTREAZĂ DOAR PARTEA DE SQL UPDATE ---

            // 1. REGULI GENERALE
            migrationBuilder.Sql("UPDATE Badges SET Icon = 'bi-bug-fill' WHERE Name LIKE '%Bug%' OR Name LIKE '%Eroare%' OR Name LIKE '%Fix%'");
            migrationBuilder.Sql("UPDATE Badges SET Icon = 'bi-trophy-fill' WHERE Name LIKE '%Expert%' OR Name LIKE '%Senior%' OR Name LIKE '%Master%'");
            migrationBuilder.Sql("UPDATE Badges SET Icon = 'bi-stopwatch-fill' WHERE Name LIKE '%Timp%' OR Name LIKE '%Rapid%' OR Name LIKE '%Deadline%'");
            migrationBuilder.Sql("UPDATE Badges SET Icon = 'bi-hand-thumbs-up-fill' WHERE Name LIKE '%Welcome%' OR Name LIKE '%Novice%' OR Name LIKE '%Start%'");
            migrationBuilder.Sql("UPDATE Badges SET Icon = 'bi-people-fill' WHERE Name LIKE '%Team%' OR Name LIKE '%Echipa%'");
            migrationBuilder.Sql("UPDATE Badges SET Icon = 'bi-check-circle-fill' WHERE Name LIKE '%Task%' OR Name LIKE '%Harnic%'");

            // 2. CANTITATE
            migrationBuilder.Sql("UPDATE Badges SET Icon = 'bi-1-circle-fill' WHERE Name LIKE '%Primul%' OR Name LIKE '%First%' OR Name LIKE '%1 Task%'");
            migrationBuilder.Sql("UPDATE Badges SET Icon = 'bi-check2-all' WHERE Name LIKE '%10 %' OR Name LIKE '%Zece%'");
            migrationBuilder.Sql("UPDATE Badges SET Icon = 'bi-collection-fill' WHERE Name LIKE '%100 %' OR Name LIKE '%Suta%'");
            migrationBuilder.Sql("UPDATE Badges SET Icon = 'bi-gem' WHERE Name LIKE '%1000 %' OR Name LIKE '%Mie%'");

            // 3. TIMP
            migrationBuilder.Sql("UPDATE Badges SET Icon = 'bi-alarm-fill' WHERE Name LIKE '%Punctual%' OR Name LIKE '%Timp%'");
            migrationBuilder.Sql("UPDATE Badges SET Icon = 'bi-lightning-charge-fill' WHERE Name LIKE '%Rapid%' OR Name LIKE '%Viteza%'");
            migrationBuilder.Sql("UPDATE Badges SET Icon = 'bi-calendar-check-fill' WHERE Name LIKE '%Deadline%' OR Name LIKE '%Termen%'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Opțional: Putem reseta la iconița default dacă dăm undo
            migrationBuilder.Sql("UPDATE Badges SET Icon = 'bi-award-fill'");
        }
    }
}
