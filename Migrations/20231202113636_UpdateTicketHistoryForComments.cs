using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheSupportTicketSystem.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTicketHistoryForComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommentId",
                table: "TicketHistories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OperationType",
                table: "TicketHistories",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "TicketHistories");

            migrationBuilder.DropColumn(
                name: "OperationType",
                table: "TicketHistories");
        }
    }
}
