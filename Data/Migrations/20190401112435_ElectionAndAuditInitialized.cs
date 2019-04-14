using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace electionDbAnalytics.Data.Migrations
{
    public partial class ElectionAndAuditInitialized : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Audit",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IPAddress = table.Column<string>(nullable: false),
                    Location = table.Column<string>(nullable: false),
                    TimeOfAction = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audit", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Elections",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Year = table.Column<string>(nullable: false),
                    Constituency_Number = table.Column<string>(nullable: false),
                    District = table.Column<string>(nullable: false),
                    Constituency = table.Column<string>(nullable: false),
                    Latitude = table.Column<float>(nullable: false),
                    Longitude = table.Column<float>(nullable: false),
                    Registered_Voters = table.Column<int>(nullable: false),
                    ValidVotes = table.Column<int>(nullable: false),
                    Voter_TurnOut_Percentage = table.Column<float>(nullable: false),
                    Winner = table.Column<string>(nullable: false),
                    RunnerUp = table.Column<string>(nullable: false),
                    WinnerVotes = table.Column<int>(nullable: false),
                    RunnerUpVotes = table.Column<int>(nullable: false),
                    MarginVotes = table.Column<int>(nullable: false),
                    MarginPercentage = table.Column<float>(nullable: false),
                    Magnitude = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elections", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Audit");

            migrationBuilder.DropTable(
                name: "Elections");
        }
    }
}
