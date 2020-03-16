using Microsoft.EntityFrameworkCore.Migrations;

namespace DistroLabCommunity.Migrations
{
    public partial class Changes6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLogin_Users_UserID",
                table: "UserLogin");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLogin",
                table: "UserLogin");

            migrationBuilder.RenameTable(
                name: "UserLogin",
                newName: "UserLogins");

            migrationBuilder.RenameIndex(
                name: "IX_UserLogin_UserID",
                table: "UserLogins",
                newName: "IX_UserLogins_UserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLogins",
                table: "UserLogins",
                column: "Login");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLogins_Users_UserID",
                table: "UserLogins",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLogins_Users_UserID",
                table: "UserLogins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLogins",
                table: "UserLogins");

            migrationBuilder.RenameTable(
                name: "UserLogins",
                newName: "UserLogin");

            migrationBuilder.RenameIndex(
                name: "IX_UserLogins_UserID",
                table: "UserLogin",
                newName: "IX_UserLogin_UserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLogin",
                table: "UserLogin",
                column: "Login");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLogin_Users_UserID",
                table: "UserLogin",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
