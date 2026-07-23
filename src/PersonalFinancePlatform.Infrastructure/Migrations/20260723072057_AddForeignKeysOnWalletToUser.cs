using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalFinancePlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKeysOnWalletToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Wallets_OwnerId",
                table: "Wallets",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_Users_OwnerId",
                table: "Wallets",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_Users_OwnerId",
                table: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_Wallets_OwnerId",
                table: "Wallets");
        }
    }
}
