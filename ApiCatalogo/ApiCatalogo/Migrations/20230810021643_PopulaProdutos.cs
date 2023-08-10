using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiCatalogo.Migrations
{
    public partial class PopulaProdutos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO produtos(Nome,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId,Descricao)" +
                                "VALUES ('Coca-Cola Diet', 5.45,'cocacola.jpg',50,now(),1,'Refrigerante de Cola 350 ml' )");

            migrationBuilder.Sql("INSERT INTO produtos(Nome,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId,Descricao)" +
                                "VALUES ('Lanche de Atum', 8.50,'atum.jpg',10,now(),2,'Lanche de Atum com maionese' )");

            migrationBuilder.Sql("INSERT INTO produtos(Nome,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId,Descricao)" +
                                "VALUES ('Pudim', 6.75,'pudim.jpg',20,now(),3,'Pudim de leite condensado 100g')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM produtos");
        }
    }
}
