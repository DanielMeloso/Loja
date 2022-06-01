using Loja.Models;
using System.Collections.Generic;
using X.PagedList;

namespace Loja.Repositories.Contracts
{
    public interface ICategoriaRepository
    {
        void Cadastrar(Categoria categoria);
        void Atualizar(Categoria categoria);
        void Excluir(int id);
        Categoria ObterCategoria(int id);
        Categoria ObterCategoria(string slug);
        IEnumerable<Categoria> ObterCategoriaRecursivo(Categoria categoriaPai);
        IEnumerable<Categoria> ObterTodasCategorias();
        IPagedList<Categoria> ObterTodasCategorias(int? pagina);
    }
}
