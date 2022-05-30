using Loja.Models.ViewModels;
using Loja.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Loja.Libraries.Component
{
    public class ProdutoListagemViewComponent : ViewComponent
    {
        private IProdutoRepository _produtoRepository;
        public ProdutoListagemViewComponent(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int? pagina = 1;
            string pesquisa = "";
            string ordenacao = "";
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

            var viewModel = new ProdutoListagemViewModel() {
                listaProduto = _produtoRepository.ObterTodosProdutos(pagina, pesquisa, ordenacao)
            };
            return View(viewModel);
        }
    }
}
