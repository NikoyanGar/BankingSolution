using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ScoringServiceAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddingClientScoresTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientScores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientId = table.Column<string>(type: "text", nullable: true),
                    Score = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientScores", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ClientScores",
                columns: new[] { "Id", "ClientId", "Score", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "C001", 750, new DateTime(2025, 10, 8, 8, 32, 34, 442, DateTimeKind.Utc).AddTicks(7534) },
                    { 2, "C002", 620, new DateTime(2025, 10, 8, 8, 32, 34, 442, DateTimeKind.Utc).AddTicks(7732) },
                    { 3, "C003", 450, new DateTime(2025, 10, 8, 8, 32, 34, 442, DateTimeKind.Utc).AddTicks(7733) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientScores");
        }
    }
}
