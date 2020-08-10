using Microsoft.EntityFrameworkCore.Migrations;

namespace PMANews.Migrations
{
    public partial class ManyMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseApplicationUser_AspNetUsers_ApplicationUserId",
                table: "CourseApplicationUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseApplicationUser",
                table: "CourseApplicationUser");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "CourseApplicationUser",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CourseApplicationUser",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseApplicationUser",
                table: "CourseApplicationUser",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CourseApplicationUser_ApplicationUserId",
                table: "CourseApplicationUser",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseApplicationUser_AspNetUsers_ApplicationUserId",
                table: "CourseApplicationUser",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseApplicationUser_AspNetUsers_ApplicationUserId",
                table: "CourseApplicationUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseApplicationUser",
                table: "CourseApplicationUser");

            migrationBuilder.DropIndex(
                name: "IX_CourseApplicationUser_ApplicationUserId",
                table: "CourseApplicationUser");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CourseApplicationUser");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "CourseApplicationUser",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseApplicationUser",
                table: "CourseApplicationUser",
                columns: new[] { "ApplicationUserId", "CourseId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CourseApplicationUser_AspNetUsers_ApplicationUserId",
                table: "CourseApplicationUser",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
