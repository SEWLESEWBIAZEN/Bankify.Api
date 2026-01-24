using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bankify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class JointTableForRoleAndClaim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleClaims_AppRoles_AppRoleId",
                table: "RoleClaims");

            migrationBuilder.RenameColumn(
                name: "ClaimString",
                table: "RoleClaims",
                newName: "UpdatedBy");

            migrationBuilder.AlterColumn<int>(
                name: "AppRoleId",
                table: "RoleClaims",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AppClaimId",
                table: "RoleClaims",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "RoleClaims",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "RecordStatus",
                table: "RoleClaims",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RegisteredBy",
                table: "RoleClaims",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "RegisteredDate",
                table: "RoleClaims",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "AppClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimString = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppClaims", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_AppClaimId",
                table: "RoleClaims",
                column: "AppClaimId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleClaims_AppClaims_AppClaimId",
                table: "RoleClaims",
                column: "AppClaimId",
                principalTable: "AppClaims",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleClaims_AppRoles_AppRoleId",
                table: "RoleClaims",
                column: "AppRoleId",
                principalTable: "AppRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleClaims_AppClaims_AppClaimId",
                table: "RoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleClaims_AppRoles_AppRoleId",
                table: "RoleClaims");

            migrationBuilder.DropTable(
                name: "AppClaims");

            migrationBuilder.DropIndex(
                name: "IX_RoleClaims_AppClaimId",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "AppClaimId",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "LastUpdateDate",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "RecordStatus",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "RegisteredBy",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "RegisteredDate",
                table: "RoleClaims");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "RoleClaims",
                newName: "ClaimString");

            migrationBuilder.AlterColumn<int>(
                name: "AppRoleId",
                table: "RoleClaims",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleClaims_AppRoles_AppRoleId",
                table: "RoleClaims",
                column: "AppRoleId",
                principalTable: "AppRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
