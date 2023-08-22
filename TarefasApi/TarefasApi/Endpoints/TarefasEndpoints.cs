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

            app.MapGet("/tarefa/{id:int}", async(GetConnection connectionGetter,int id) =>
            {
                using var con = await connectionGetter();
                var tarefa = con.Get<Tarefa>(id);

                if(tarefa is null)
                    return Results.NotFound("Tarefa não encontrada");
                
                return Results.Ok(tarefa);

            });

        }
    }
}
