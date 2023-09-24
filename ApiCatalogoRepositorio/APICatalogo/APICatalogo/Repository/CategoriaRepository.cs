using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public IEnumerable<Categoria> GetCategoriasProduto(AppDbContext context)
        {
            AppDbContext _context = context;
            return Get().Include(x => x.Produtos);
        }
    }
}
