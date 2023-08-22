using Dapper.Contrib.Extensions;
using TarefasApi.Data;
using static TarefasApi.Data.TarefaContext;

namespace TarefasApi.Endpoints
{
    public static class TarefasEndpoints
    {
        public static void MapTarefasEndpoints(this WebApplication app)
        {
            app.MapGet("/", () => $"Bem-Vindo a API Tarefas {DateTime.Now}");

            app.MapGet("/tarefas", async(GetConnection connectionGetter) =>
            {
                using var con = await connectionGetter();
                var tarefas = con.GetAll<Tarefa>().ToList();
                
                if(tarefas is null)
                    return Results.NotFound();

                return Results.Ok(tarefas);
            });

            app.MapGet("/tarefa/{id}", async(GetConnection connectionGetter,int id) =>
            {
                using var con = await connectionGetter();
                var tarefa = con.Get<Tarefa>(id);

                if(tarefa is null)
                    return Results.NotFound("Tarefa não encontrada");
                
                return Results.Ok(tarefa);

            });

            app.MapPost("/tarefas", async(GetConnection connectionGetter, Tarefa tarefa) =>
            {
                using var con = await connectionGetter();
                if(tarefa is null)
                    return Results.BadRequest("Tarefa Nula");

                var id = con.Insert<Tarefa>(tarefa);
                return Results.Created($"/tarefa/{id}",tarefa);
            });

            app.MapPut("/tarefas", async (GetConnection connectionGetter, Tarefa tarefa) =>
            {
                using var con = await connectionGetter();
                var id = con.Update(tarefa);
                if(tarefa is null)
                    return Results.NotFound("Tarefa não encontrada");
                return Results.Ok(id);
            });
            
            app.MapDelete("/tarefa/{id}", async(GetConnection connectionGetter,int id) =>
            {
                using var con = await connectionGetter();
                var tarefa = con.Get<Tarefa>(id);
                if(tarefa is null)
                    return Results.NotFound("Tarefa não encontrada");
                
                await con.DeleteAsync(tarefa);
                return Results.Ok(tarefa);
            });
        }
    }
}
