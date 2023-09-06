using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinApiCatalogo.ApiEndpoints;
using MinApiCatalogo.AppServiceExtensions;
using MinApiCatalogo.Context;
using MinApiCatalogo.Models;
using MinApiCatalogo.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. //ConfigureServices no Startup
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.AddApiSwagger();

var app = builder.Build();

var environment = app.Environment;

app.UseExceptionHandling(environment)
    .UseSwaggerMiddleware()
    .UseAppCors();


// Configure the HTTP request pipeline.//Configure
if (app.Environment.IsDevelopment())
{
    
}


app.MapAutenticacaoEndpoints();
app.MapGet("/", () => "Catálogo de produtos - 2023").ExcludeFromDescription();
app.MapCategoriasEndpoints();
app.MapProdutosEndpoints();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
