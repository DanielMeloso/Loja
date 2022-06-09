using AutoMapper;
using Loja.Libraries.CarrinhoCompra;
using Loja.Libraries.Lang;
using Loja.Models.ProdutoAgregador;
using Loja.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Loja.Controllers
{
    public class CarrinhoCompraController : Controller
    {
        private CarrinhoCompra _carrinhoCompra;
        private IProdutoRepository _produtoRepository;
        private IMapper _mapper;
        public CarrinhoCompraController(CarrinhoCompra carrinhoCompra, IProdutoRepository produtoRepository, IMapper mapper)
        {
            _carrinhoCompra = carrinhoCompra;
            _produtoRepository = produtoRepository;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var produtosCarrinho = _carrinhoCompra.Consultar();
            var produtoItemCompleto = new List<ProdutoItem>();
            foreach(var produtoCarrinho in produtosCarrinho)
            {
                var produto = _produtoRepository.ObterProduto(produtoCarrinho.Id);

                // passa todas as informações do objeto Produto, para ProdutoItem
                var produtoItem = _mapper.Map<ProdutoItem>(produto);
                produtoItem.QuantidadeProdutoCarrinho = produtoCarrinho.QuantidadeProdutoCarrinho;

                produtoItemCompleto.Add(produtoItem);

            }
            return View(produtoItemCompleto);
        }

        public IActionResult AdicionarItem(int id)
        {
            var produto = _produtoRepository.ObterProduto(id);

            if (produto == null)
            {
                return View("NaoExisteItem");
            } else
            {
                var item = new ProdutoItem()
                {
                    Id = id,
                    QuantidadeProdutoCarrinho = 1
                };

                _carrinhoCompra.Cadastrar(item);

                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult AlterarQuantidade(int id, int quantidade)
        {
            var produto = _produtoRepository.ObterProduto(id);
            if (quantidade < 1)
            {
                return BadRequest(new { mensagem = Mensagem.MSG_E007 });
            } else if (quantidade > produto.Quantidade)
            {
                return BadRequest(new { mensagem = Mensagem.MSG_E008 });
            } else
            {
                var item = new ProdutoItem() { Id = id, QuantidadeProdutoCarrinho = quantidade };
                _carrinhoCompra.Atualizar(item);
                return Ok(new { mensagem = Mensagem.MSG_S001});
            }
            
        }

        public IActionResult RemoverItem(int id)
        {
            _carrinhoCompra.Remover(new ProdutoItem()
            {
                Id = id
            });
            return RedirectToAction(nameof(Index));
        }
    }
}
