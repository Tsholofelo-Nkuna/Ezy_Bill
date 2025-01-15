using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClientManagement.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentAndPaymentDateToInvoicePaymentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "InvoicesPayments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "InvoicesPayments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "InvoicesPayments");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "InvoicesPayments");
        }
    }
}
