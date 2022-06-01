using Loja.Models;
using Loja.Models.ViewModels;
using Loja.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Loja.Libraries.Component
{
    public class ProdutoListagemViewComponent : ViewComponent
    {
        private IProdutoRepository _produtoRepository;
        private ICategoriaRepository _categoriaRepository;
        public ProdutoListagemViewComponent(IProdutoRepository produtoRepository, ICategoriaRepository categoriaRepository)
        {
            _produtoRepository = produtoRepository;
            _categoriaRepository = categoriaRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int? pagina = 1;
            string pesquisa = "";
            string ordenacao = "";
            IEnumerable<Categoria> categorias = null;
            if (HttpContext.Request.Query.ContainsKey("pagina"))
            {
                pagina = int.Parse(HttpContext.Request.Query["pagina"]);
            }

            if (HttpContext.Request.Query.ContainsKey("pesquisa"))
            {
                pesquisa = HttpContext.Request.Query["pesquisa"].ToString();
            }

            if (HttpContext.Request.Query.ContainsKey("ordenacao"))
            {
                ordenacao = HttpContext.Request.Query["ordenacao"].ToString();
            }

            if (ViewContext.RouteData.Values.ContainsKey("slug"))
            {
                var slug = ViewContext.RouteData.Values["slug"].ToString();
                var categoriaPrincipal = _categoriaRepository.ObterCategoria(slug);
                categorias = _categoriaRepository.ObterCategoriaRecursivo(categoriaPrincipal);
            }

            var viewModel = new ProdutoListagemViewModel() {
                listaProduto = _produtoRepository.ObterTodosProdutos(pagina, pesquisa, ordenacao, categorias)
            };
            return View(viewModel);
        }
    }
}
