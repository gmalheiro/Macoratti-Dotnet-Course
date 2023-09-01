using Microsoft.EntityFrameworkCore;
using MinApiCatalogo.Context;
using MinApiCatalogo.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. //ConfigureServices no Startup
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options => 
                                            options.
                                            UseMySql(connectionString,ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

// Configure the HTTP request pipeline.//Configure
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/",() => "Catálogo de produtos - 2023");

app.MapPost("/categorias", async (AppDbContext db,Categoria categoria ) =>
{
    if (categoria is null)
        return Results.BadRequest("Categoria nula");

    db.Categoria?.AddAsync(categoria);
    
    await db?.SaveChangesAsync()!;
    
    return Results.Created($"/categorias/{categoria.CategoriaId}",categoria);
});

app.MapGet("/categorias", async (AppDbContext db) => await db?.Categoria?.ToListAsync()!); ;

app.MapGet("/categorias/{id:int}", async (AppDbContext db, int id) =>
{
    return await db.Categoria!.FindAsync(id)
    is Categoria categoria
        ? Results.Ok(categoria)
        : Results.NotFound("Categoria não encontrada") ;
});

app.MapPut("/categorias/{id:int}", async (int id,AppDbContext db, Categoria categoria) =>
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

    if(categoria is null)
        return Results.NotFound("Categoria não encontrada");
    
    db.Categoria.Remove(categoria);
    
    await db.SaveChangesAsync();
    
    return Results.Ok(categoria);
});

app.UseHttpsRedirection();

app.Run();
