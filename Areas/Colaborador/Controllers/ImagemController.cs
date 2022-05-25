using Loja.Libraries.Arquivo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Loja.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    public class ImagemController : Controller
    {
        [HttpPost]
        public IActionResult Armazenar(IFormFile file)
        {
            var caminho = GerenciadorArquivo.CadastrarImagemProduto(file);

            if (!string.IsNullOrEmpty(caminho))
            {
                return Ok(new { caminho = caminho });
            }
            return new StatusCodeResult(500);
        }

        public IActionResult Deletar(string caminho)
        {
            if (GerenciadorArquivo.DeletarImagemProduto(caminho))
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
