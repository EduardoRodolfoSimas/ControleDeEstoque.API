using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleDeEstoque.API.Migrations
{
    /// <inheritdoc />
    public partial class DeNovo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vendas_Pagamentos_MetodoPagamento",
                table: "Vendas");

            migrationBuilder.RenameColumn(
                name: "MetodoPagamento",
                table: "Vendas",
                newName: "MetodoPagamentoId");

            migrationBuilder.RenameIndex(
                name: "IX_Vendas_MetodoPagamento",
                table: "Vendas",
                newName: "IX_Vendas_MetodoPagamentoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendas_Pagamentos_MetodoPagamentoId",
                table: "Vendas",
                column: "MetodoPagamentoId",
                principalTable: "Pagamentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vendas_Pagamentos_MetodoPagamentoId",
                table: "Vendas");

            migrationBuilder.RenameColumn(
                name: "MetodoPagamentoId",
                table: "Vendas",
                newName: "MetodoPagamento");

            migrationBuilder.RenameIndex(
                name: "IX_Vendas_MetodoPagamentoId",
                table: "Vendas",
                newName: "IX_Vendas_MetodoPagamento");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendas_Pagamentos_MetodoPagamento",
                table: "Vendas",
                column: "MetodoPagamento",
                principalTable: "Pagamentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
