using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheSupportTicketSystem.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddPriorityToTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Tickets");
        }
    }
}
