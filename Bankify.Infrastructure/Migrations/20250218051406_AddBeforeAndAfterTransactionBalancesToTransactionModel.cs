using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bankify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBeforeAndAfterTransactionBalancesToTransactionModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "AmmountTransfered",
                table: "Transfers",
                type: "decimal(38,10)",
                precision: 38,
                scale: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<decimal>(
                name: "BalanceAfterTransaction",
                table: "TransactionLogs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BalanceBeforeTransaction",
                table: "TransactionLogs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "InterestRate",
                table: "AccountTypes",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "Accounts",
                type: "decimal(38,10)",
                precision: 38,
                scale: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BalanceAfterTransaction",
                table: "TransactionLogs");

            migrationBuilder.DropColumn(
                name: "BalanceBeforeTransaction",
                table: "TransactionLogs");

            migrationBuilder.AlterColumn<decimal>(
                name: "AmmountTransfered",
                table: "Transfers",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(38,10)",
                oldPrecision: 38,
                oldScale: 10);

            migrationBuilder.AlterColumn<decimal>(
                name: "InterestRate",
                table: "AccountTypes",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldPrecision: 5,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "Accounts",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(38,10)",
                oldPrecision: 38,
                oldScale: 10);
        }
    }
}
