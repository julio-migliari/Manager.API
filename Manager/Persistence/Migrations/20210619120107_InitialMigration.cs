using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Manager.Persistence.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vendedores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cpf = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Endereco = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendedores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tamanho = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VendedorForeignKey = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produtos_Vendedores_VendedorForeignKey",
                        column: x => x.VendedorForeignKey,
                        principalTable: "Vendedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeProduto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TamanhoProduto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorProduto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NomeCliente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TelCliente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnderecoCliente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetodoPagamento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VendedorForeignKey = table.Column<int>(type: "int", nullable: false),
                    ProdutoForeignKey = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pedidos_Produtos_ProdutoForeignKey",
                        column: x => x.ProdutoForeignKey,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pedidos_Vendedores_VendedorForeignKey",
                        column: x => x.VendedorForeignKey,
                        principalTable: "Vendedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_ProdutoForeignKey",
                table: "Pedidos",
                column: "ProdutoForeignKey");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_VendedorForeignKey",
                table: "Pedidos",
                column: "VendedorForeignKey");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_VendedorForeignKey",
                table: "Produtos",
                column: "VendedorForeignKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "Vendedores");
        }
    }
}
