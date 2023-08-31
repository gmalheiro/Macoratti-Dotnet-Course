using Microsoft.EntityFrameworkCore;
using MinApiCatalogo.Context;

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

app.UseHttpsRedirection();

app.Run();
