using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        [Route("/ListarProdutos")]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _context?.Produtos.ToList();

            if (produtos is null)
            {
                return NotFound("Produtos não encontrados...");
            }
         
            return Ok(_context?.Produtos.ToList());
        }
        // /produtos/primeiro
        [HttpGet("primeiro")]
        public ActionResult<Produto> GetPrimeiroProduto()
        {
            var produto = _context?.Produtos.FirstOrDefault();

            if (produto is null)
            {
                return NotFound("Produto não encontrado...");
            }
            return Ok(produto);
        }

        //[HttpGet("{id:int}",Name = "ObterProduto")]
        [HttpGet("{id:int}",Name = "ObterProduto")]
        public ActionResult <Produto> Get(int id)
        {
            var produto = _context?.Produtos.FirstOrDefault(produto => produto.ProdutoId == id);

            if (produto is null)
            {
                return NotFound("Produto não encontrado...");
            }
            return Ok(produto);
        }

        [HttpPost]
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
