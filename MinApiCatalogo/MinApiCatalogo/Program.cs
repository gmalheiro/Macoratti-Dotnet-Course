using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MinApiCatalogo.Context;
using MinApiCatalogo.Models;
using MinApiCatalogo.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. //ConfigureServices no Startup
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
                                            options.
                                            UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddSingleton<ITokenService>(new TokenService());

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer(options =>
                 {
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuer = true,
                         ValidateAudience = true,
                         ValidateLifetime = true,
                         ValidateIssuerSigningKey = true,

                         ValidIssuer = builder.Configuration["Jwt:Issuer"],
                         ValidAudience = builder.Configuration["Jwt:Audience"],
                         IssuerSigningKey = new SymmetricSecurityKey
                         (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                     };
                 });

var app = builder.Build();

// Configure the HTTP request pipeline.//Configure
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//endpoint para login
app.MapPost("/login", [AllowAnonymous] (UserModel userModel, ITokenService tokenService) =>
{
    if (userModel == null)
    {
        return Results.BadRequest("Login Inválido");
    }
    if (userModel.UserName == "DotnetMalheiro" && userModel.Password == "numsey#123")
    {
        var tokenString = tokenService.GerarToken(app.Configuration["Jwt:Key"],
            app.Configuration["Jwt:Issuer"],
            app.Configuration["Jwt:Audience"],
            userModel);
        return Results.Ok(new { token = tokenString });
    }
    else
    {
        return Results.BadRequest("Login Inv�lido");
    }
}).Produces(StatusCodes.Status400BadRequest)
              .Produces(StatusCodes.Status200OK)
              .WithName("Login")
              .WithTags("Autenticacao");

app.MapGet("/", () => "Catálogo de produtos - 2023").ExcludeFromDescription();

app.MapPost("/categorias", async (AppDbContext db, Categoria categoria) =>
{
    if (categoria is null)
        return Results.BadRequest("Categoria nula");

    db.Categoria?.AddAsync(categoria);

    await db?.SaveChangesAsync()!;

    return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);
});

app.MapGet("/categorias", async (AppDbContext db) => await db?.Categoria?.ToListAsync()!).RequireAuthorization(); ;

app.MapGet("/categorias/{id:int}", async (AppDbContext db, int id) =>
{
    return await db.Categoria!.FindAsync(id)
    is Categoria categoria
        ? Results.Ok(categoria)
        : Results.NotFound("Categoria não encontrada");
});

app.MapPut("/categorias/{id:int}", async (int id, AppDbContext db, Categoria categoria) =>
{
    if (id != categoria.CategoriaId)
        return Results.BadRequest();

    var categoriaDb = await db.Categoria!.FindAsync(id);
    if (categoriaDb is null)
        return Results.NotFound("Categoria não encontrada");

    categoriaDb.Nome = categoria.Nome;
    categoriaDb.Descricao = categoria.Descricao;

    await db.SaveChangesAsync();

    return Results.Ok(categoriaDb);
});

app.MapDelete("/categorias/{id:int}", async (AppDbContext db, int id) =>
{
    var categoria = await db.Categoria!.FindAsync(id);

    if (categoria is null)
        return Results.NotFound("Categoria não encontrada");

    db.Categoria.Remove(categoria);

    await db.SaveChangesAsync();

    return Results.Ok(categoria);
});

//-------------------------endpoints para produtos---------------------------------
app.MapPost("/produtos", async (AppDbContext db, Produto produto) =>
{
    if (produto is null)
        return Results.BadRequest("Produto nulo");

    db.Produto?.AddAsync(produto);

    await db?.SaveChangesAsync()!;

    return Results.Created($"/produto/{produto.ProdutoId}", produto);
});

app.MapGet("/produtos", async (AppDbContext db) => await db?.Produto?.ToListAsync()!);

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

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.Run();
