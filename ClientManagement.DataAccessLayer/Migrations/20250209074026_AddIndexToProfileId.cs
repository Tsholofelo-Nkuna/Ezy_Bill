using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClientManagement.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexToProfileId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Profiles_ProfileId",
                table: "Profiles",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProfileId",
                table: "Products",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoicesProducts_ProfileId",
                table: "InvoicesProducts",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoicesPayments_ProfileId",
                table: "InvoicesPayments",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_ProfileId",
                table: "Invoices",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_ProfileId",
                table: "Contacts",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_ProfileId",
                table: "Clients",
                column: "ProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Profiles_ProfileId",
                table: "Profiles");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProfileId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_InvoicesProducts_ProfileId",
                table: "InvoicesProducts");

            migrationBuilder.DropIndex(
                name: "IX_InvoicesPayments_ProfileId",
                table: "InvoicesPayments");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_ProfileId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_ProfileId",
                table: "Contacts");

            migrationBuilder.DropIndex(
                name: "IX_Clients_ProfileId",
                table: "Clients");
        }
    }
}
