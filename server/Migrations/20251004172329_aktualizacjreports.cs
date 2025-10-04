using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class aktualizacjreports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Verifications_Reports_ReportId",
                table: "Verifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Verifications_Users_UserId",
                table: "Verifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Verifications",
                table: "Verifications");

            migrationBuilder.DropIndex(
                name: "IX_Verifications_UserId_ReportId",
                table: "Verifications");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ResolvedAt",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Reports");

            migrationBuilder.RenameTable(
                name: "Verifications",
                newName: "Verification");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Reports",
                newName: "IncidentType");

            migrationBuilder.RenameIndex(
                name: "IX_Verifications_ReportId",
                table: "Verification",
                newName: "IX_Verification_ReportId");

            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "Reports",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LocationName",
                table: "Reports",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "Reports",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Verification",
                table: "Verification",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Verification_UserId",
                table: "Verification",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Verification_Reports_ReportId",
                table: "Verification",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Verification_Users_UserId",
                table: "Verification",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Verification_Reports_ReportId",
                table: "Verification");

            migrationBuilder.DropForeignKey(
                name: "FK_Verification_Users_UserId",
                table: "Verification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Verification",
                table: "Verification");

            migrationBuilder.DropIndex(
                name: "IX_Verification_UserId",
                table: "Verification");

            migrationBuilder.RenameTable(
                name: "Verification",
                newName: "Verifications");

            migrationBuilder.RenameColumn(
                name: "IncidentType",
                table: "Reports",
                newName: "Status");

            migrationBuilder.RenameIndex(
                name: "IX_Verification_ReportId",
                table: "Verifications",
                newName: "IX_Verifications_ReportId");

            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "Reports",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<string>(
                name: "LocationName",
                table: "Reports",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "Reports",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Reports",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResolvedAt",
                table: "Reports",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "Reports",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Reports",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Verifications",
                table: "Verifications",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Verifications_UserId_ReportId",
                table: "Verifications",
                columns: new[] { "UserId", "ReportId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Verifications_Reports_ReportId",
                table: "Verifications",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Verifications_Users_UserId",
                table: "Verifications",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
