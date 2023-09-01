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

app.MapGet("/",() => "Cat�logo de produtos - 2023");

app.MapPost("/categorias", async (AppDbContext db,Categoria categoria ) =>
{
    if (categoria is null)
        return Results.BadRequest("Categoria nula");
    db.Categoria?.AddAsync(categoria);
    await db?.SaveChangesAsync()!;
    return Results.Created($"/categorias/{categoria.CategoriaId}",categoria);
});

app.UseHttpsRedirection();

app.Run();
