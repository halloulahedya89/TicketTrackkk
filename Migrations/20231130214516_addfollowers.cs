using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheSupportTicketSystem.Web.Migrations
{
    /// <inheritdoc />
    public partial class addfollowers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketFollower_AspNetUsers_UserId",
                table: "TicketFollower");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketFollower_Tickets_TicketId",
                table: "TicketFollower");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketFollower",
                table: "TicketFollower");

            migrationBuilder.RenameTable(
                name: "TicketFollower",
                newName: "TicketFollowers");

            migrationBuilder.RenameIndex(
                name: "IX_TicketFollower_TicketId",
                table: "TicketFollowers",
                newName: "IX_TicketFollowers_TicketId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketFollowers",
                table: "TicketFollowers",
                columns: new[] { "UserId", "TicketId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TicketFollowers_AspNetUsers_UserId",
                table: "TicketFollowers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketFollowers_Tickets_TicketId",
                table: "TicketFollowers",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "TicketId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketFollowers_AspNetUsers_UserId",
                table: "TicketFollowers");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketFollowers_Tickets_TicketId",
                table: "TicketFollowers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketFollowers",
                table: "TicketFollowers");

            migrationBuilder.RenameTable(
                name: "TicketFollowers",
                newName: "TicketFollower");

            migrationBuilder.RenameIndex(
                name: "IX_TicketFollowers_TicketId",
                table: "TicketFollower",
                newName: "IX_TicketFollower_TicketId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketFollower",
                table: "TicketFollower",
                columns: new[] { "UserId", "TicketId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TicketFollower_AspNetUsers_UserId",
                table: "TicketFollower",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketFollower_Tickets_TicketId",
                table: "TicketFollower",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "TicketId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
