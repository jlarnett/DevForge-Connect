using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevForge_Connect.Migrations
{
    /// <inheritdoc />
    public partial class teamInvites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TeamInvites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecipientUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamInvites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamInvites_AspNetUsers_RecipientUserId",
                        column: x => x.RecipientUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TeamInvites_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TeamInvites_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeamInvites_RecipientUserId",
                table: "TeamInvites",
                column: "RecipientUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamInvites_StatusId",
                table: "TeamInvites",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamInvites_TeamId",
                table: "TeamInvites",
                column: "TeamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamInvites");
        }
    }
}
