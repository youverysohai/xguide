using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XGuideSQLiteDB.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Manipulators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manipulators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Role = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Calibrations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    ManipulatorId = table.Column<int>(type: "INTEGER", nullable: true),
                    Orientation = table.Column<int>(type: "INTEGER", nullable: true),
                    Speed = table.Column<double>(type: "REAL", nullable: true),
                    Acceleration = table.Column<double>(type: "REAL", nullable: true),
                    MotionDelay = table.Column<int>(type: "INTEGER", nullable: true),
                    XOffset = table.Column<int>(type: "INTEGER", nullable: true),
                    YOffset = table.Column<int>(type: "INTEGER", nullable: true),
                    MMPerPixel = table.Column<double>(type: "REAL", nullable: false),
                    CRZOffset = table.Column<double>(type: "REAL", nullable: false),
                    CYOffset = table.Column<double>(type: "REAL", nullable: false),
                    CXOffset = table.Column<double>(type: "REAL", nullable: false),
                    Procedure = table.Column<string>(type: "TEXT", nullable: true),
                    Mode = table.Column<bool>(type: "INTEGER", nullable: false),
                    JointRotationAngle = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calibrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Calibrations_Manipulators_ManipulatorId",
                        column: x => x.ManipulatorId,
                        principalTable: "Manipulators",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Calibrations_ManipulatorId",
                table: "Calibrations",
                column: "ManipulatorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Calibrations");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Manipulators");
        }
    }
}
