using Loja.Models.ProdutoAgregador;
using Loja.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Loja.Controllers
{
    public class ProdutoController : Controller
    {
        private ICategoriaRepository _categoriaRepository;
        private IProdutoRepository _produtoRepository;

        public ProdutoController(ICategoriaRepository categoriaRepository, IProdutoRepository produtoRepository)
        {
            _categoriaRepository = categoriaRepository;
            _produtoRepository = produtoRepository;
        }

        [Route("Produto/Categoria/{slug}")]
        [HttpGet]
        public IActionResult ListagemCategoria(string slug)
        {
            return View(_categoriaRepository.ObterCategoria(slug));
        }

        [HttpGet]
        public IActionResult Visualizar(int id)
        {
            Produto produto = _produtoRepository.ObterProduto(id);
            return View(produto);
        }

        
    }
}
