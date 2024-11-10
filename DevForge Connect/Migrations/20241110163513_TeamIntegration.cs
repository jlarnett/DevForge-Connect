using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevForge_Connect.Migrations
{
    /// <inheritdoc />
    public partial class TeamIntegration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AIGeneratedSummary",
                table: "ProjectSubmissions");

            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "ProjectSubmissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "ProjectBids",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSubmissions_TeamId",
                table: "ProjectSubmissions",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectBids_TeamId",
                table: "ProjectBids",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectBids_Teams_TeamId",
                table: "ProjectBids",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectSubmissions_Teams_TeamId",
                table: "ProjectSubmissions",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectBids_Teams_TeamId",
                table: "ProjectBids");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectSubmissions_Teams_TeamId",
                table: "ProjectSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_ProjectSubmissions_TeamId",
                table: "ProjectSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_ProjectBids_TeamId",
                table: "ProjectBids");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "ProjectSubmissions");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "ProjectBids");

            migrationBuilder.AddColumn<string>(
                name: "AIGeneratedSummary",
                table: "ProjectSubmissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
