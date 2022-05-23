using Loja.Libraries.Filtro;
using Loja.Libraries.Lang;
using Loja.Models.Constants;
using Loja.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Loja.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    [ColaboradorAutorizacao]
    public class ClienteController : Controller
    {
        private IClienteRepository _clienteRepository;

        public ClienteController(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public IActionResult Index(int? pagina, string pesquisa)
        {
            IPagedList clientes = _clienteRepository.ObterTodosClientes(pagina, pesquisa);
            return View(clientes);
        }

        [ValidateHttpReferer]
        public IActionResult AtivarDesativar(int id)
        {
            var cliente = _clienteRepository.ObterCliente(id);

            cliente.Situacao = cliente.Situacao == SituacaoConstant.Ativo ? cliente.Situacao = SituacaoConstant.Desativado : SituacaoConstant.Ativo;
            _clienteRepository.Atualizar(cliente);

            TempData["MSG_S"] = Mensagem.MSG_S002;

            return RedirectToAction("Index");
        }
    }
}
