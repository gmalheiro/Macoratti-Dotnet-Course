using TarefasApi.Endpoints;
using TarefasApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(o => o.AddPolicy("CorsRule", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));

builder.AddPersistence();

var app = builder.Build();

app.UseCors("CorsRule");

app.MapTarefasEndpoints();

app.Run();
