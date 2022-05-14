using Microsoft.EntityFrameworkCore.Migrations;

namespace TP.Data.Migrations
{
    public partial class useridnoavaliacaoparastring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Avaliacoes_AspNetUsers_UserId1",
                table: "Avaliacoes");

            migrationBuilder.DropIndex(
                name: "IX_Avaliacoes_UserId1",
                table: "Avaliacoes");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Avaliacoes");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Avaliacoes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacoes_UserId",
                table: "Avaliacoes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Avaliacoes_AspNetUsers_UserId",
                table: "Avaliacoes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Avaliacoes_AspNetUsers_UserId",
                table: "Avaliacoes");

            migrationBuilder.DropIndex(
                name: "IX_Avaliacoes_UserId",
                table: "Avaliacoes");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Avaliacoes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Avaliacoes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacoes_UserId1",
                table: "Avaliacoes",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Avaliacoes_AspNetUsers_UserId1",
                table: "Avaliacoes",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
