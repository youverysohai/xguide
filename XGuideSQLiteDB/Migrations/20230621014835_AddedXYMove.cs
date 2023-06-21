using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XGuideSQLiteDB.Migrations
{
    /// <inheritdoc />
    public partial class AddedXYMove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "XMove",
                table: "Calibrations",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "YMove",
                table: "Calibrations",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "XMove",
                table: "Calibrations");

            migrationBuilder.DropColumn(
                name: "YMove",
                table: "Calibrations");
        }
    }
}
