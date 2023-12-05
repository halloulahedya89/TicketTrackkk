using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheSupportTicketSystem.Web.Migrations
{
    /// <inheritdoc />
    public partial class updateTicketModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_AssignedToUserId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_UserId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_AssignedToUserId",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Tickets",
                newName: "AssignedToId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_UserId",
                table: "Tickets",
                newName: "IX_Tickets_AssignedToId");

            migrationBuilder.AlterColumn<string>(
                name: "AssignedToUserId",
                table: "Tickets",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Tickets",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CreatedById",
                table: "Tickets",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_AssignedToId",
                table: "Tickets",
                column: "AssignedToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_CreatedById",
                table: "Tickets",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_AssignedToId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_CreatedById",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_CreatedById",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "AssignedToId",
                table: "Tickets",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_AssignedToId",
                table: "Tickets",
                newName: "IX_Tickets_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "AssignedToUserId",
                table: "Tickets",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_AssignedToUserId",
                table: "Tickets",
                column: "AssignedToUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_AssignedToUserId",
                table: "Tickets",
                column: "AssignedToUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_UserId",
                table: "Tickets",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
