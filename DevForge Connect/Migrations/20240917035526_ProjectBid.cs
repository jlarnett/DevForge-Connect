using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevForge_Connect.Migrations
{
    /// <inheritdoc />
    public partial class ProjectBid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectBids",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfferAmount = table.Column<decimal>(type: "money", nullable: false),
                    FinishDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectBids", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectBids_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ProjectBids_ProjectSubmissions_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "ProjectSubmissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectBids_ProjectId",
                table: "ProjectBids",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectBids_UserId",
                table: "ProjectBids",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectBids");
        }
    }
}
