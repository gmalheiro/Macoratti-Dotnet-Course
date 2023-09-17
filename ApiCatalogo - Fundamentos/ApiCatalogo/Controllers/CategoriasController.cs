using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {

        private readonly AppDbContext? _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(AppDbContext context, IConfiguration configuration, ILogger<CategoriasController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet("autor")]
        public string GetAutor() 
        {
            var autor = _configuration["autor"];
            return $"Autor: {autor}";
        }

        [HttpGet("saudacao/{nome}")]
        public ActionResult<string> GetSaudacao([FromServices] IMeuServico meuServico, string nome)
        {
            return meuServico.saudacao(nome);
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            _logger.LogInformation("================== GET Api/produtos");
            //return Ok(_context?.Categorias.Include(p=> p.Produtos).ToList());
            return Ok(_context?.Categorias.Include(p=> p.Produtos).Where(c => c.CategoriaId <= 5).ToList());
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {

            try
            {
                var categorias = _context?.Categorias.AsNoTracking().ToList();
                if (categorias is null)
                {
                    return NotFound("Categorias não encontradas...");
                }
                return Ok(categorias);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "Ocorreu um erro ao tratar a sua solicitação");
            }
        }


        [HttpGet("id:int",Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            try
            {
                var categoria = _context?.Categorias.FirstOrDefault(categoria => categoria.CategoriaId == id);

                if (categoria is null)
                {
                    return NotFound("Categoria não encontrada");
                }

                return Ok(categoria);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  ("Ocorreu um erro ao tratar a sua solicitação"));
            }
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            if (categoria is null)
                return BadRequest("Categoria nula");

            _context?.Categorias.Add(categoria);
            _context?.SaveChanges();

            return CreatedAtRoute("ObterCategoria",
                new Categoria { CategoriaId = categoria.CategoriaId},categoria);

        }

        [HttpPut("id:int")]
        public ActionResult Put (int id, Categoria categoria)
        {

            if (id != categoria.CategoriaId)
                return NotFound("Categoria não encontrada...");

            _context!.Entry(categoria).State = EntityState.Modified;

            _context?.SaveChanges(true);

            return Ok(categoria);

        }

        [HttpDelete("id:int")]
        public ActionResult<Categoria> Delete(int id)
        {
            var categoria = _context?.Categorias.FirstOrDefault(c=> c.CategoriaId == id);

            _context?.Remove(categoria!);
            _context?.SaveChanges();

            return Ok(categoria);
        }

    }
}
