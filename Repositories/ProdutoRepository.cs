using Loja.Database;
using Loja.Models;
using Loja.Repositories.Contracts;
using Microsoft.Extensions.Configuration;
using System.Linq;
using X.PagedList;

namespace Loja.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private IConfiguration _configuration;
        private LojaContext _banco;

        public ProdutoRepository(IConfiguration configuration, LojaContext banco)
        {
            _configuration = configuration;
            _banco = banco;
        }

        public void Atualizar(Produto produto)
        {
            _banco.Update(produto);
            _banco.SaveChanges();
        }

        public void Cadastrar(Produto produto)
        {
            _banco.Produtos.Add(produto);
            _banco.SaveChanges();
        }

        public void Excluir(int id)
        {
            var produto = ObterProduto(id);
            _banco.Remove(produto);
            _banco.SaveChanges();
        }

        public Produto ObterProduto(int id)
        {
            return _banco.Produtos.Find(id);
        }

        public IPagedList<Produto> ObterTodosProdutos(int? pagina, string pesquisa)
        {
            int NumeroPagina = pagina ?? 1;
            var bancoProdutos = _banco.Produtos.AsQueryable();

            if (!string.IsNullOrEmpty(pesquisa))
            {
                bancoProdutos = bancoProdutos.Where(x => x.Nome.Contains(pesquisa.Trim() ));
            }

            return bancoProdutos.ToPagedList(NumeroPagina, _configuration.GetValue<int>("RegistroPorPagina"));
        }
    }
}
