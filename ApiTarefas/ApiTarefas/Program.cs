using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options=> options.UseInMemoryDatabase("TarefasDB"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/",() => "Olá mundo");

app.MapGet("/Nomes", async () => await new HttpClient().GetStringAsync("https://gerador-nomes.wolan.net/nome/aleatorio")) ;

app.MapGet("frases", async () => await new HttpClient().GetStringAsync("https://ron-swanson-quotes.herokuapp.com/v2/quotes"));

app.MapGet("/Tarefas", async (AppDbContext db) => await db.Tarefas.ToListAsync());

app.MapPost("/CriarTarefa", async(Tarefa tarefa, AppDbContext db) => 
{
    db?.Tarefas.AddAsync(tarefa);
    await db!.SaveChangesAsync();
    return Results.Created($"/Tarefas/{tarefa.Id}",tarefa);
});

app.MapGet("/TarefaPorId/{id:int}", async(int id, AppDbContext db) => await db.Tarefas.FindAsync(id) is Tarefa tarefa ? Results.Ok(tarefa) : Results.NotFound());

app.MapGet("/Tarefas/Concluidas", async (AppDbContext db) => await db.Tarefas.Where(t => t.IsConcluida).ToListAsync());

app.Run();

class Tarefa
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public bool IsConcluida { get; set; }
}

class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {}

    public DbSet<Tarefa> Tarefas => Set<Tarefa>();

}