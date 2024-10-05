using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevForge_Connect.Migrations
{
    /// <inheritdoc />
    public partial class statusTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectBids_ProjectSubmissions_ProjectId",
                table: "ProjectBids");

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "ProjectSubmissions",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "ProjectBids",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "ProjectBids",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSubmissions_StatusId",
                table: "ProjectSubmissions",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectBids_StatusId",
                table: "ProjectBids",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectBids_ProjectSubmissions_ProjectId",
                table: "ProjectBids",
                column: "ProjectId",
                principalTable: "ProjectSubmissions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectBids_Statuses_StatusId",
                table: "ProjectBids",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectSubmissions_Statuses_StatusId",
                table: "ProjectSubmissions",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectBids_ProjectSubmissions_ProjectId",
                table: "ProjectBids");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectBids_Statuses_StatusId",
                table: "ProjectBids");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectSubmissions_Statuses_StatusId",
                table: "ProjectSubmissions");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropIndex(
                name: "IX_ProjectSubmissions_StatusId",
                table: "ProjectSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_ProjectBids_StatusId",
                table: "ProjectBids");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "ProjectSubmissions");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "ProjectBids");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "ProjectBids",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectBids_ProjectSubmissions_ProjectId",
                table: "ProjectBids",
                column: "ProjectId",
                principalTable: "ProjectSubmissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
