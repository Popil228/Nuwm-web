using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project1.Migrations
{
    public partial class StudSubg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Subgroups_SubgroupId",
                table: "Students");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Subgroups_SubgroupId",
                table: "Students",
                column: "SubgroupId",
                principalTable: "Subgroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Subgroups_SubgroupId",
                table: "Students");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Subgroups_SubgroupId",
                table: "Students",
                column: "SubgroupId",
                principalTable: "Subgroups",
                principalColumn: "Id");
        }
    }
}
