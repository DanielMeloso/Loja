using Loja.Database;
using Loja.Models;
using Loja.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using X.PagedList;

namespace Loja.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly IConfiguration _configuration;
        private LojaContext _banco;
        public CategoriaRepository(LojaContext banco, IConfiguration configuration)
        {
            _banco = banco;
            _configuration = configuration;
        }
        public void Atualizar(Categoria categoria)
        {
            _banco.Update(categoria);
            _banco.SaveChanges();
        }

        public void Cadastrar(Categoria categoria)
        {
            _banco.Categorias.Add(categoria);
            _banco.SaveChanges();
        }

        public void Excluir(int id)
        {
            var categoria = ObterCategoria(id);
            _banco.Remove(categoria);
            _banco.SaveChanges();
        }

        public Categoria ObterCategoria(int id)
        {
            return _banco.Categorias.Find(id);
        }

        public Categoria ObterCategoria(string slug)
        {
            return _banco.Categorias.FirstOrDefault(x => x.Slug == slug);
        }

        private List<Categoria> listaCategoriaRecursiva = new List<Categoria>();
        private List<Categoria> categorias;
        public IEnumerable<Categoria> ObterCategoriaRecursivo(Categoria categoriaPai)
        {
            if (categorias == null)
            {
                categorias = ObterTodasCategorias().ToList();
            }

            if (!listaCategoriaRecursiva.Exists(x => x.Id == categoriaPai.Id))
            {
                listaCategoriaRecursiva.Add(categoriaPai);
            }

            var listaCategoriaFilho = categorias.Where(x => x.CategoriaPaiId == categoriaPai.Id);
            if (listaCategoriaFilho.Count() > 0)
            {
                listaCategoriaRecursiva.AddRange(listaCategoriaFilho.ToList());
                foreach (var categoriaFilho in listaCategoriaFilho)
                {
                    ObterCategoriaRecursivo(categoriaFilho);
                }
            }

            return listaCategoriaRecursiva;
        }

        

        public IPagedList<Categoria> ObterTodasCategorias(int? pagina)
        {
            int numeroPagina = pagina ?? 1;
            return _banco.Categorias
                .Include(x => x.CategoriaPai)
                .ToPagedList(numeroPagina, _configuration.GetValue<int>("RegistroPorPagina"));
        }

        public IEnumerable<Categoria> ObterTodasCategorias()
        {
            return _banco.Categorias;
        }
    }
}
