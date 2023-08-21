using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using TarefasApi.Data;
using static TarefasApi.Data.TarefaContext;

namespace TarefasApi.Endpoints
{
    public static class TarefasEndpoints
    {
        public static void MapTarefasEndpoints(this WebApplication app)
        {
            app.MapGet("/",() =>$"Bem-vndo a API Tarefas {DateTime.Now}");

            app.MapGet("/Tarefas", async(GetConnection connectionGetter) =>
            {
                using var con = await connectionGetter();
                var tarefas = con.GetAll<Tarefa>().ToList();

                if(tarefas is null)
                    return Results.NotFound("Tarefas não encontradas");

                return Results.Ok<ICollection<Tarefa>>(tarefas);

                

            });
        }
    }
}