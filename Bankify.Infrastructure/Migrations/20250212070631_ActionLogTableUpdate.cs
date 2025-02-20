using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bankify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ActionLogTableUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdateDate",
                table: "ActionLogs");

            migrationBuilder.DropColumn(
                name: "RecordStatus",
                table: "ActionLogs");

            migrationBuilder.DropColumn(
                name: "RegisteredBy",
                table: "ActionLogs");

            migrationBuilder.DropColumn(
                name: "RegisteredDate",
                table: "ActionLogs");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ActionLogs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "ActionLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "RecordStatus",
                table: "ActionLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RegisteredBy",
                table: "ActionLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "RegisteredDate",
                table: "ActionLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "ActionLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
