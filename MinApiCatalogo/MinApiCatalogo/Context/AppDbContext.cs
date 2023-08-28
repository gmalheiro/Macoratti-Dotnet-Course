using Microsoft.EntityFrameworkCore;
using MinApiCatalogo.Models;

namespace MinApiCatalogo.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options){}
        public DbSet<Categoria>? Categoria { get; set; }
        public DbSet<Produto>? Produto { get; set; }
    }
}
