using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project1.Migrations
{
    public partial class connections_PS_PT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_AspNetUsers_PersonId1",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_AspNetUsers_PersonId1",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_PersonId1",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Students_PersonId1",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "PersonId1",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "PersonId1",
                table: "Students");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PersonId1",
                table: "Teachers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonId1",
                table: "Students",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_PersonId1",
                table: "Teachers",
                column: "PersonId1",
                unique: true,
                filter: "[PersonId1] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Students_PersonId1",
                table: "Students",
                column: "PersonId1",
                unique: true,
                filter: "[PersonId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_AspNetUsers_PersonId1",
                table: "Students",
                column: "PersonId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_AspNetUsers_PersonId1",
                table: "Teachers",
                column: "PersonId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
