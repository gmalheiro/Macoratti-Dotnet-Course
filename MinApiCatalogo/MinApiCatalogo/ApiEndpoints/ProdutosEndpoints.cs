using Microsoft.EntityFrameworkCore;
using MinApiCatalogo.Context;
using MinApiCatalogo.Models;

namespace MinApiCatalogo.ApiEndpoints
{
    public static class ProdutosEndpoints
    {
        public static void MapProdutosEndpoints(this WebApplication app)
        {
            app.MapPost("/produtos", async (AppDbContext db, Produto produto) =>
            {
                if (produto is null)
                    return Results.BadRequest("Produto nulo");

                db.Produto?.AddAsync(produto);

                await db?.SaveChangesAsync()!;

                return Results.Created($"/produto/{produto.ProdutoId}", produto);
            });

            app.MapGet("/produtos", async (AppDbContext db) => await db?.Produto?.ToListAsync()!).WithTags("Produtos").RequireAuthorization();

            app.MapGet("/produtos/{id:int}", async (AppDbContext db, int id) =>
            {
                return await db.FindAsync<Produto>(id)
                       is Produto produto
                       ? Results.Ok(produto)
                       : Results.NotFound("Produto não encontrado");
            });

            app.MapPut("/produtos/{id:int}", async (int id, AppDbContext db, Produto produto) =>
            {
                if (id != produto.ProdutoId)
                    return Results.BadRequest();

                var produtoDb = await db.Produto!.FindAsync(id);
                if (produtoDb is null)
                    return Results.NotFound("Produto não encontrada");

                produtoDb.Nome = produto.Nome;
                produtoDb.Descricao = produto.Descricao;
                produtoDb.Preco = produto.Preco;
                produtoDb.Imagem = produto.Imagem;
                produtoDb.DataCompra = produto.DataCompra;
                produtoDb.Estoque = produto.Estoque;
                produtoDb.CategoriaId = produto.CategoriaId;

                await db.SaveChangesAsync();

                return Results.Ok(produtoDb);
            });

            app.MapDelete("/produtos/{id:int}", async (AppDbContext db, int id) =>
            {
                var produto = await db.Produto!.FindAsync(id);

                if (produto is null)

                    db.Produto.Remove(produto!);

                await db.SaveChangesAsync();

                return Results.Ok(produto);
            });
        }
    }
}
