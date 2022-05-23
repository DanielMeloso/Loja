using Loja.Models;
using Microsoft.AspNetCore.Mvc;

namespace Loja.Controllers
{
    public class ProdutoController : Controller
    {
        public IActionResult Visualizar()
        {
            Produto produto = GetProduto();
            return View(produto);
        }

        private Produto GetProduto()
        {
            return new Produto()
            {
                Id = 1,
                Nome = "Video Game",
                Descricao = "Descrição do Produto",
                Valor = 2000.00M
            };
        }
    }
}
