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
            // --- AM COMENTAT CREAREA COLOANEI PENTRU CĂ EXISTĂ DEJA ---
            // migrationBuilder.AddColumn<string>(name: "Icon", table: "Badges", ...);

            // === POPULARE AUTOMATĂ A ICONIȚELOR ===

            // 1. CANTITATE (Numere)
            // Primul Task -> Cifra 1
            migrationBuilder.Sql("UPDATE Badges SET Icon = 'bi-1-circle-fill' WHERE Name LIKE '%Primul%' OR Name LIKE '%First%' OR Name LIKE '%1 Task%'");

            // 10 Task-uri -> Cifra 1 pătrat / Listă
            migrationBuilder.Sql("UPDATE Badges SET Icon = 'bi-collection-fill' WHERE Name LIKE '%10 %' OR Name LIKE '%Zece%'");

            // 100 Task-uri -> Stea în cerc
            migrationBuilder.Sql("UPDATE Badges SET Icon = 'bi-stars' WHERE Name LIKE '%100 %' OR Name LIKE '%Suta%'");

            // 1000 Task-uri -> Diamant
            migrationBuilder.Sql("UPDATE Badges SET Icon = 'bi-gem' WHERE Name LIKE '%1000 %' OR Name LIKE '%Mie%'");

            // 2. TIMP / VITEZĂ
            // Rapid / Viteza -> Fulger
            migrationBuilder.Sql("UPDATE Badges SET Icon = 'bi-lightning-charge-fill' WHERE Name LIKE '%Rapid%' OR Name LIKE '%Viteza%' OR Name LIKE '%Fast%'");

            // Punctual / Deadline -> Ceas / Calendar
            migrationBuilder.Sql("UPDATE Badges SET Icon = 'bi-alarm-fill' WHERE Name LIKE '%Punctual%' OR Name LIKE '%Timp%' OR Name LIKE '%Deadline%'");

            // 3. SPECIAL / BUGURI
            // Bug-uri -> Gândac
            migrationBuilder.Sql("UPDATE Badges SET Icon = 'bi-bug-fill' WHERE Name LIKE '%Bug%' OR Name LIKE '%Fix%'");

            // Expert / Senior -> Trofeu
            migrationBuilder.Sql("UPDATE Badges SET Icon = 'bi-trophy-fill' WHERE Name LIKE '%Expert%' OR Name LIKE '%Senior%' OR Name LIKE '%Master%'");

            // Începător -> Like / Mână
            migrationBuilder.Sql("UPDATE Badges SET Icon = 'bi-hand-thumbs-up-fill' WHERE Name LIKE '%Welcome%' OR Name LIKE '%Novice%' OR Name LIKE '%Start%'");

            // Echipă -> Oameni
            migrationBuilder.Sql("UPDATE Badges SET Icon = 'bi-people-fill' WHERE Name LIKE '%Team%' OR Name LIKE '%Echipa%'");

            // Default pentru restul (ca să nu rămână nimic gol)
            migrationBuilder.Sql("UPDATE Badges SET Icon = 'bi-award-fill' WHERE Icon IS NULL OR Icon = ''");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Opțional: Putem reseta la iconița default dacă dăm undo
            migrationBuilder.Sql("UPDATE Badges SET Icon = 'bi-award-fill'");
        }
    }
}
