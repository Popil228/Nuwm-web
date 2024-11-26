using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project1.Data.Migrations
{
    public partial class redisign : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Persons_PersonId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_PersonId",
                table: "Students");

            migrationBuilder.AlterColumn<string>(
                name: "PersonId",
                table: "Students",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Students_PersonId",
                table: "Students",
                column: "PersonId",
                unique: true,
                filter: "[PersonId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Persons_PersonId",
                table: "Students",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Persons_PersonId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_PersonId",
                table: "Students");

            migrationBuilder.AlterColumn<string>(
                name: "PersonId",
                table: "Students",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_PersonId",
                table: "Students",
                column: "PersonId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Persons_PersonId",
                table: "Students",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
