using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevForge_Connect.Migrations
{
    /// <inheritdoc />
    public partial class user : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamInvites_AspNetUsers_RecipientUserId",
                table: "TeamInvites");

            migrationBuilder.DropIndex(
                name: "IX_TeamInvites_RecipientUserId",
                table: "TeamInvites");

            migrationBuilder.DropColumn(
                name: "RecipientUserId",
                table: "TeamInvites");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TeamInvites",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeamInvites_UserId",
                table: "TeamInvites",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamInvites_AspNetUsers_UserId",
                table: "TeamInvites",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamInvites_AspNetUsers_UserId",
                table: "TeamInvites");

            migrationBuilder.DropIndex(
                name: "IX_TeamInvites_UserId",
                table: "TeamInvites");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TeamInvites",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecipientUserId",
                table: "TeamInvites",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeamInvites_RecipientUserId",
                table: "TeamInvites",
                column: "RecipientUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamInvites_AspNetUsers_RecipientUserId",
                table: "TeamInvites",
                column: "RecipientUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
