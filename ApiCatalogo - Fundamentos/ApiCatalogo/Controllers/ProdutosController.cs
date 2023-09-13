using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext? _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        // /ListarProdutos
        //[Route("/ListarProdutos")]
        public async Task<ActionResult<IEnumerable<Produto>>> GetAsync()
        {
            var produtos = await _context?.Produtos.AsNoTracking().ToListAsync()!;
            return Ok(produtos);
        }
        // /produtos/primeiro
        //[HttpGet("primeiro")]
        //[HttpGet("teste")]
        //[HttpGet("/primeiro")]
        [HttpGet("{valor:alpha:length(5)}")]
        public ActionResult<Produto> GetPrimeiroProduto()
        {
            var produto = _context?.Produtos.FirstOrDefault();

            if (produto is null)
            {
                return NotFound("Produto não encontrado...");
            }
            return Ok(produto);
        }

        //[HttpGet("{id}/{param2}",Name = "ObterProduto")]
        //[HttpGet("{id}/{nome=Caderno}",Name = "ObterProduto")]
        [HttpGet("{id:int:min(1)}",Name = "ObterProduto")]
        public async Task<ActionResult <Produto>> GetByIdAsync([FromQuery]int id)
        { 
            var produto = await _context?.Produtos?.AsNoTracking()?.FirstOrDefaultAsync(p => p.ProdutoId == id)!;

            if (produto is null)
            {
                return NotFound("Produto não encontrado...");
            }
            return Ok(produto);
        }

        [HttpPost]
        // /api/produtos
        public ActionResult Post(Produto produto)
        {
            if(produto is null)
                return BadRequest("Produto nulo");

            _context?.Produtos.Add(produto);
            _context?.SaveChanges();


            return new CreatedAtRouteResult("ObterProduto",
                new {id = produto.ProdutoId},produto);


        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                return BadRequest("Produto não encontrado");
            }

            _context!.Entry(produto).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges(true);

            return Ok(produto);

        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _context?.Produtos.FirstOrDefault(produto => produto.ProdutoId == id);

            if (produto is null)
            {
                return BadRequest("Produto não encontrado");
            }

            _context?.Produtos.Remove(produto);
            _context?.SaveChanges();

            return Ok(produto);

        }


        


    }
}
