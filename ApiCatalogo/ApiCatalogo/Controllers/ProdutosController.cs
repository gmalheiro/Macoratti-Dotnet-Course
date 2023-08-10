﻿using ApiCatalogo.Context;
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
        [Route("/ListarProutos")]
        public ActionResult<IEnumerable<Produto>> ListarProdutos()
        {
            var produtos = _context?.Produtos.ToList();

            if (produtos is null)
            {
                return NotFound("Produtos não encontrados...");
            }
         
            return Ok(_context?.Produtos.ToList());
        }
    }
}
