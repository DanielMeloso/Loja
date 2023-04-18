using AutoMapper;
using Loja.Libraries.CarrinhoCompra;
using Loja.Libraries.Gerenciador.Frete;
using Loja.Libraries.Lang;
using Loja.Models;
using Loja.Models.Constants;
using Loja.Models.ProdutoAgregador;
using Loja.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Loja.Controllers
{
    public class CarrinhoCompraController : Controller
    {
        private CarrinhoCompra _carrinhoCompra;
        private IProdutoRepository _produtoRepository;
        private IMapper _mapper;
        private WSCorreiosCalcularFrete _wsCorreios;
        private CalcularPacote _calcularPacote;
        public CarrinhoCompraController(CarrinhoCompra carrinhoCompra, IProdutoRepository produtoRepository, IMapper mapper, WSCorreiosCalcularFrete wsCorreios, CalcularPacote calcularPacote)
        {
            _carrinhoCompra = carrinhoCompra;
            _produtoRepository = produtoRepository;
            _mapper = mapper;
            _wsCorreios = wsCorreios;
            _calcularPacote = calcularPacote;
        }

        public IActionResult Index()
        {
            List<ProdutoItem> produtoItemCompleto = CarregarProdutoDB();
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

        public async Task<IActionResult> CalcularFrete(int cepDestino)
        {
            try
            {
                var produtos = CarregarProdutoDB();
                var pacotes = _calcularPacote.CalcularPacotesDeprodutos(produtos);
                var valorPAC = await _wsCorreios.CalcularFrete(cepDestino.ToString(), TipoFreteConstant.PAC, pacotes);
                var valorSEDEX = await _wsCorreios.CalcularFrete(cepDestino.ToString(), TipoFreteConstant.SEDEX, pacotes);
                var valorSEDEX10 = await _wsCorreios.CalcularFrete(cepDestino.ToString(), TipoFreteConstant.SEDEX10, pacotes);

                List<ValorPrazoFrete> lista = new List<ValorPrazoFrete>();
                
                if (valorPAC != null) lista.Add(valorPAC);
                if (valorSEDEX!= null) lista.Add(valorSEDEX);
                if (valorSEDEX10!= null) lista.Add(valorSEDEX10);

                return Ok(lista);
            } catch (Exception e)
            {
                return BadRequest(e.Message);
            }


        }

        private List<ProdutoItem> CarregarProdutoDB()
        {
            var produtosCarrinho = _carrinhoCompra.Consultar();
            var produtoItemCompleto = new List<ProdutoItem>();
            foreach (var produtoCarrinho in produtosCarrinho)
            {
                var produto = _produtoRepository.ObterProduto(produtoCarrinho.Id);

                // passa todas as informações do objeto Produto, para ProdutoItem
                var produtoItem = _mapper.Map<ProdutoItem>(produto);
                produtoItem.QuantidadeProdutoCarrinho = produtoCarrinho.QuantidadeProdutoCarrinho;

                produtoItemCompleto.Add(produtoItem);

            }

            return produtoItemCompleto;
        }
    }
}
