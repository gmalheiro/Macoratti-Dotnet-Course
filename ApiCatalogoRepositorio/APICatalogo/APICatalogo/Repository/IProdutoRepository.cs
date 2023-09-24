using APICatalogo.Models;

namespace APICatalogo.Repository;

public interface IProdutoRepository : IProdutoRepository<Produto> 
{
    IEnumerable<Produto> GetProdutoPorPreco();
}