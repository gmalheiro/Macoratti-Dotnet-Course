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
        //[Route("/ListarProdutos")]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _context?.Produtos.ToList();

            if (produtos is null)
            {
                return NotFound("Produtos não encontrados...");
            }
         
            return Ok(_context?.Produtos.ToList());
        }

        //[HttpGet("{id:int}",Name = "ObterProduto")]
        [HttpGet("{id:int}")]
        public ActionResult <Produto> Get(int id)
        {
            var produto = _context?.Produtos.FirstOrDefault(produto => produto.ProdutoId == id);

            if (produto is null)
            {
                return NotFound("Produto não encontrado...");
            }
            return Ok(produto);
        }

    }
}
