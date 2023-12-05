using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheSupportTicketSystem.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddUserToTicketHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "TicketHistories",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TicketHistories_UserId",
                table: "TicketHistories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketHistories_AspNetUsers_UserId",
                table: "TicketHistories",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketHistories_AspNetUsers_UserId",
                table: "TicketHistories");

            migrationBuilder.DropIndex(
                name: "IX_TicketHistories_UserId",
                table: "TicketHistories");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TicketHistories");
        }
    }
}
