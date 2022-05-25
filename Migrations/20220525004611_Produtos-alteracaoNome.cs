using Microsoft.EntityFrameworkCore.Migrations;

namespace Loja.Migrations
{
    public partial class ProdutosalteracaoNome : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Decimal",
                table: "Produtos",
                newName: "Quantidade");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantidade",
                table: "Produtos",
                newName: "Decimal");
        }
    }
}
