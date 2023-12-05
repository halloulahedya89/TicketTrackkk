using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheSupportTicketSystem.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddUserToTicketHistory2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "ChangedByUserId",
                table: "TicketHistories",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TicketHistories_ChangedByUserId",
                table: "TicketHistories",
                column: "ChangedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketHistories_AspNetUsers_ChangedByUserId",
                table: "TicketHistories",
                column: "ChangedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketHistories_AspNetUsers_ChangedByUserId",
                table: "TicketHistories");

            migrationBuilder.DropIndex(
                name: "IX_TicketHistories_ChangedByUserId",
                table: "TicketHistories");

            migrationBuilder.AlterColumn<string>(
                name: "ChangedByUserId",
                table: "TicketHistories",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

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
    }
}
