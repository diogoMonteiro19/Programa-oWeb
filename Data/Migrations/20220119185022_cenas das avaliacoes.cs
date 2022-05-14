using Microsoft.EntityFrameworkCore.Migrations;

namespace TP.Data.Migrations
{
    public partial class cenasdasavaliacoes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Avaliacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Avaliacoes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AvalicoesClientes",
                columns: table => new
                {
                    AvalicaoClienteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comentario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pontuacao = table.Column<double>(type: "float", nullable: false),
                    ClienteId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    GestorId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvalicoesClientes", x => x.AvalicaoClienteId);
                    table.ForeignKey(
                        name: "FK_AvalicoesClientes_AspNetUsers_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AvalicoesClientes_AspNetUsers_GestorId",
                        column: x => x.GestorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacoes_UserId1",
                table: "Avaliacoes",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_AvalicoesClientes_ClienteId",
                table: "AvalicoesClientes",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_AvalicoesClientes_GestorId",
                table: "AvalicoesClientes",
                column: "GestorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Avaliacoes_AspNetUsers_UserId1",
                table: "Avaliacoes",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Avaliacoes_AspNetUsers_UserId1",
                table: "Avaliacoes");

            migrationBuilder.DropTable(
                name: "AvalicoesClientes");

            migrationBuilder.DropIndex(
                name: "IX_Avaliacoes_UserId1",
                table: "Avaliacoes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Avaliacoes");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Avaliacoes");
        }
    }
}
