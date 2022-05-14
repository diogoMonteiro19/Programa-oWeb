using Microsoft.EntityFrameworkCore.Migrations;

namespace TP.Data.Migrations
{
    public partial class gestorligaoaimovel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GestorId",
                table: "Imoveis",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Imoveis_GestorId",
                table: "Imoveis",
                column: "GestorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Imoveis_AspNetUsers_GestorId",
                table: "Imoveis",
                column: "GestorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Imoveis_AspNetUsers_GestorId",
                table: "Imoveis");

            migrationBuilder.DropIndex(
                name: "IX_Imoveis_GestorId",
                table: "Imoveis");

            migrationBuilder.DropColumn(
                name: "GestorId",
                table: "Imoveis");
        }
    }
}
