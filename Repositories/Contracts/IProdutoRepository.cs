using Loja.Models;
using System.Collections.Generic;
using X.PagedList;

namespace Loja.Repositories.Contracts
{
    public interface IProdutoRepository
    {
        void Cadastrar(Produto produto);
        void Atualizar(Produto produto);
        void Excluir(int id);
        Produto ObterProduto(int id);
        IPagedList<Produto> ObterTodosProdutos(int? pagina, string pesquisa);
        IPagedList<Produto> ObterTodosProdutos(int? pagina, string pesquisa, string ordenacao, IEnumerable<Categoria> categorias);
    }
}
