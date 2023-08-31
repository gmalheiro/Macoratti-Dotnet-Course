using Microsoft.EntityFrameworkCore;
using MinApiCatalogo.Models;

namespace MinApiCatalogo.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {}
        public DbSet<Categoria>? Categoria { get; set; }
        public DbSet<Produto>? Produto { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            //Categoria
            mb.Entity<Categoria>().HasKey(c => c.CategoriaId);
            mb.Entity<Categoria>().Property(c => c.Nome)
                                  .HasMaxLength(100)
                                   .IsRequired();
            mb.Entity<Categoria>().Property(c => c.Descricao).HasMaxLength(150).IsRequired();

            //Produto
            mb.Entity<Produto>().HasKey(p => p.ProdutoId );
            mb.Entity<Produto>().Property(p => p.Nome).HasMaxLength(100).IsRequired();
            mb.Entity<Produto>().Property(p => p.Descricao).HasMaxLength(150).IsRequired();
            mb.Entity<Produto>().Property(p => p.Imagem).HasMaxLength(100).IsRequired();
            mb.Entity<Produto>().Property(p => p.Preco).HasPrecision(14,2);

            //relacionamento
            mb.Entity<Produto>()
                .HasOne<Categoria>(c => c.Categoria)
                    .WithMany(p => p.Produtos)
                    .HasForeignKey(c => c.CategoriaId);
        }
    }
}
