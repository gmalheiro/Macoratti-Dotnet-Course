using MinApiCatalogo.ApiEndpoints;
using MinApiCatalogo.AppServiceExtensions;

var builder = WebApplication.CreateBuilder(args);


builder.AddApiSwagger();
builder.AddPersistence();
builder.AddAuthenticationJwt();
builder.Services.AddCors();

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
