using Loja.Libraries.Arquivo;
using Loja.Libraries.Filtro;
using Loja.Libraries.Lang;
using Loja.Models;
using Loja.Models.ProdutoAgregador;
using Loja.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace Loja.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    [ColaboradorAutorizacao]
    public class ProdutoController : Controller
    {
        private IProdutoRepository _produtoRepository;
        private ICategoriaRepository _categoriaRepository;
        private IImagemRepository _imagemRepository;
        public ProdutoController (IProdutoRepository produtoRepository, ICategoriaRepository categoriaRepository, IImagemRepository imagemRepository)
        {
            _produtoRepository = produtoRepository;
            _categoriaRepository = categoriaRepository;
            _imagemRepository = imagemRepository;
        }
        public IActionResult Index(int? pagina, string pesquisa)
        {
            var produtos = _produtoRepository.ObterTodosProdutos(pagina, pesquisa);
            return View(produtos);
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            ViewBag.Categorias = _categoriaRepository.ObterTodasCategorias().Select(x => new SelectListItem(x.Nome,x.Id.ToString()));
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(Produto produto)
        {
            if (ModelState.IsValid)
            {
                _produtoRepository.Cadastrar(produto);
                var listaImagensDef = GerenciadorArquivo.MoverImagensProduto(new List<string>(Request.Form["imagem"]), produto.Id);
                _imagemRepository.CadastrarImagens(listaImagensDef, produto.Id);

                TempData["MSG_S"] = Mensagem.MSG_S001;
                return RedirectToAction(nameof(Index));
            } else
            {
                ViewBag.Categorias = _categoriaRepository.ObterTodasCategorias().Select(x => new SelectListItem(x.Nome, x.Id.ToString()));
                produto.Imagens = new List<string>(Request.Form["imagem"])
                    .Where(x => x.Trim().Length > 0)
                    .Select(x => new Imagem() { Caminho = x }).ToList();
                return View(produto);
            }
        }

        [HttpGet]
        public IActionResult Atualizar(int id)
        {
            ViewBag.Categorias = _categoriaRepository.ObterTodasCategorias().Select(x => new SelectListItem(x.Nome, x.Id.ToString()));
            Produto produto = _produtoRepository.ObterProduto(id);
            return View(produto);
        }

        [HttpPost]
        public IActionResult Atualizar(Produto produto, int id)
        {
            if (ModelState.IsValid)
            {
                _produtoRepository.Atualizar(produto);
                var listaImagensDef = GerenciadorArquivo.MoverImagensProduto(new List<string>(Request.Form["imagem"]), produto.Id);

                _imagemRepository.ExcluirImagensProduto(produto.Id);
                _imagemRepository.CadastrarImagens(listaImagensDef, produto.Id);
                TempData["MSG_S"] = Mensagem.MSG_S002;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.Categorias = _categoriaRepository.ObterTodasCategorias().Select(x => new SelectListItem(x.Nome, x.Id.ToString()));
                produto.Imagens = new List<string>(Request.Form["imagem"])
                    .Where(x => x.Trim().Length > 0)
                    .Select(x => new Imagem() { Caminho = x }).ToList();
                return View(produto);
            }
        }

        [HttpGet]
        [ValidateHttpReferer]
        public IActionResult Excluir(int id)
        {
            var produto = _produtoRepository.ObterProduto(id);
            GerenciadorArquivo.DeletarImagensProduto(produto.Imagens.ToList());
            _imagemRepository.ExcluirImagensProduto(id);
            _produtoRepository.Excluir(id);

            TempData["MSG_S"] = Mensagem.MSG_S003;
            return RedirectToAction(nameof(Index));
        }
    }
}
