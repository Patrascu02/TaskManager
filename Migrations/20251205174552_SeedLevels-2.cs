using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskManager.Migrations
{
    /// <inheritdoc />
    public partial class SeedLevels2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Levels",
                columns: new[] { "LevelId", "LevelName", "MaxXp", "MinXp" },
                values: new object[,]
                {
                    { 1, "Level 1", 99, 0 },
                    { 2, "Level 2", 199, 100 },
                    { 3, "Level 3", 299, 200 },
                    { 4, "Level 4", 399, 300 },
                    { 5, "Level 5", 499, 400 },
                    { 6, "Level 6", 599, 500 },
                    { 7, "Level 7", 699, 600 },
                    { 8, "Level 8", 799, 700 },
                    { 9, "Level 9", 899, 800 },
                    { 10, "Level 10", 999, 900 },
                    { 11, "Level 11", 1099, 1000 },
                    { 12, "Level 12", 1199, 1100 },
                    { 13, "Level 13", 1299, 1200 },
                    { 14, "Level 14", 1399, 1300 },
                    { 15, "Level 15", 1499, 1400 },
                    { 16, "Level 16", 1599, 1500 },
                    { 17, "Level 17", 1699, 1600 },
                    { 18, "Level 18", 1799, 1700 },
                    { 19, "Level 19", 1899, 1800 },
                    { 20, "Level 20", 1999, 1900 },
                    { 21, "Level 21", 2099, 2000 },
                    { 22, "Level 22", 2199, 2100 },
                    { 23, "Level 23", 2299, 2200 },
                    { 24, "Level 24", 2399, 2300 },
                    { 25, "Level 25", 2499, 2400 },
                    { 26, "Level 26", 2599, 2500 },
                    { 27, "Level 27", 2699, 2600 },
                    { 28, "Level 28", 2799, 2700 },
                    { 29, "Level 29", 2899, 2800 },
                    { 30, "Level 30", 2999, 2900 },
                    { 31, "Level 31", 3099, 3000 },
                    { 32, "Level 32", 3199, 3100 },
                    { 33, "Level 33", 3299, 3200 },
                    { 34, "Level 34", 3399, 3300 },
                    { 35, "Level 35", 3499, 3400 },
                    { 36, "Level 36", 3599, 3500 },
                    { 37, "Level 37", 3699, 3600 },
                    { 38, "Level 38", 3799, 3700 },
                    { 39, "Level 39", 3899, 3800 },
                    { 40, "Level 40", 3999, 3900 },
                    { 41, "Level 41", 4099, 4000 },
                    { 42, "Level 42", 4199, 4100 },
                    { 43, "Level 43", 4299, 4200 },
                    { 44, "Level 44", 4399, 4300 },
                    { 45, "Level 45", 4499, 4400 },
                    { 46, "Level 46", 4599, 4500 },
                    { 47, "Level 47", 4699, 4600 },
                    { 48, "Level 48", 4799, 4700 },
                    { 49, "Level 49", 4899, 4800 },
                    { 50, "Level 50", 4999, 4900 },
                    { 51, "Level 51", 5099, 5000 },
                    { 52, "Level 52", 5199, 5100 },
                    { 53, "Level 53", 5299, 5200 },
                    { 54, "Level 54", 5399, 5300 },
                    { 55, "Level 55", 5499, 5400 },
                    { 56, "Level 56", 5599, 5500 },
                    { 57, "Level 57", 5699, 5600 },
                    { 58, "Level 58", 5799, 5700 },
                    { 59, "Level 59", 5899, 5800 },
                    { 60, "Level 60", 5999, 5900 },
                    { 61, "Level 61", 6099, 6000 },
                    { 62, "Level 62", 6199, 6100 },
                    { 63, "Level 63", 6299, 6200 },
                    { 64, "Level 64", 6399, 6300 },
                    { 65, "Level 65", 6499, 6400 },
                    { 66, "Level 66", 6599, 6500 },
                    { 67, "Level 67", 6699, 6600 },
                    { 68, "Level 68", 6799, 6700 },
                    { 69, "Level 69", 6899, 6800 },
                    { 70, "Level 70", 6999, 6900 },
                    { 71, "Level 71", 7099, 7000 },
                    { 72, "Level 72", 7199, 7100 },
                    { 73, "Level 73", 7299, 7200 },
                    { 74, "Level 74", 7399, 7300 },
                    { 75, "Level 75", 7499, 7400 },
                    { 76, "Level 76", 7599, 7500 },
                    { 77, "Level 77", 7699, 7600 },
                    { 78, "Level 78", 7799, 7700 },
                    { 79, "Level 79", 7899, 7800 },
                    { 80, "Level 80", 7999, 7900 },
                    { 81, "Level 81", 8099, 8000 },
                    { 82, "Level 82", 8199, 8100 },
                    { 83, "Level 83", 8299, 8200 },
                    { 84, "Level 84", 8399, 8300 },
                    { 85, "Level 85", 8499, 8400 },
                    { 86, "Level 86", 8599, 8500 },
                    { 87, "Level 87", 8699, 8600 },
                    { 88, "Level 88", 8799, 8700 },
                    { 89, "Level 89", 8899, 8800 },
                    { 90, "Level 90", 8999, 8900 },
                    { 91, "Level 91", 9099, 9000 },
                    { 92, "Level 92", 9199, 9100 },
                    { 93, "Level 93", 9299, 9200 },
                    { 94, "Level 94", 9399, 9300 },
                    { 95, "Level 95", 9499, 9400 },
                    { 96, "Level 96", 9599, 9500 },
                    { 97, "Level 97", 9699, 9600 },
                    { 98, "Level 98", 9799, 9700 },
                    { 99, "Level 99", 9899, 9800 },
                    { 100, "Level 100", 9999, 9900 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 100);
        }
    }
}
