﻿using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {

        private readonly AppDbContext? _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {

            var categorias = _context?.Categorias.ToList();
            //var categorias = _context?.Categorias.Include(p => p.Produtos).Where(c=> c.CategoriaId <= 5).ToList() ;
            if (categorias is null)
            {
                return NotFound("Categorias não encontradas...");
            }


            return Ok(categorias);
        }


        [HttpGet("id:int",Name = "ObterCategoria")]
        public ActionResult Get(int id)
        {
            var categoria = _context?.Categorias.FirstOrDefault(categoria => categoria.CategoriaId == id);

            if (categoria is null)
            {
                return NotFound("Categoria não encontrada");
            }

            return Ok(categoria);
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

            _context!.Entry(categoria).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            _context?.SaveChanges(true);

            return Ok(categoria);

        }

        [HttpDelete("id:int")]
        public ActionResult Delete(int id)
        {
            var categoria = _context?.Categorias.FirstOrDefault(c=> c.CategoriaId == id);

            _context?.Remove(categoria!);
            _context?.SaveChanges();

            return Ok(categoria);
        }

    }
}
