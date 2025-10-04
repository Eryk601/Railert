using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class InitialTransportSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RideId",
                table: "Reports",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Rides",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TransportType = table.Column<int>(type: "integer", nullable: false),
                    LineNumber = table.Column<string>(type: "text", nullable: false),
                    StartStation = table.Column<string>(type: "text", nullable: false),
                    EndStation = table.Column<string>(type: "text", nullable: false),
                    DistanceKm = table.Column<double>(type: "double precision", nullable: false),
                    ScheduledDeparture = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ScheduledArrival = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DelayMinutes = table.Column<int>(type: "integer", nullable: false),
                    IsCancelled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rides", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_RideId",
                table: "Reports",
                column: "RideId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Rides_RideId",
                table: "Reports",
                column: "RideId",
                principalTable: "Rides",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Rides_RideId",
                table: "Reports");

            migrationBuilder.DropTable(
                name: "Rides");

            migrationBuilder.DropIndex(
                name: "IX_Reports_RideId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "RideId",
                table: "Reports");
        }
    }
}
