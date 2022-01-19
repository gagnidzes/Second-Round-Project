using Microsoft.EntityFrameworkCore.Migrations;

namespace BankApi.Migrations
{
    public partial class ModifyAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_UserDatas_UserDataId",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_UserDataId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "UserDataId",
                table: "Accounts");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Accounts",
                type: "int",
                nullable: true,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UserId",
                table: "Accounts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_UserDatas_UserId",
                table: "Accounts",
                column: "UserId",
                principalTable: "UserDatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_UserDatas_UserId",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_UserId",
                table: "Accounts");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "UserDataId",
                table: "Accounts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UserDataId",
                table: "Accounts",
                column: "UserDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_UserDatas_UserDataId",
                table: "Accounts",
                column: "UserDataId",
                principalTable: "UserDatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
