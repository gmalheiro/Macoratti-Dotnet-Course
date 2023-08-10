using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiCatalogo.Migrations
{
    public partial class PopulaCategorias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO categorias(Nome,ImagemUrl) values ('Bebidas','bebidas.jpg')");
            migrationBuilder.Sql("INSERT INTO categorias(Nome,ImagemUrl) values ('Lanches','lanches.jpg')");
            migrationBuilder.Sql("INSERT INTO categorias(Nome,ImagemUrl) values ('Sobremesas','sobremesas.jpg')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
