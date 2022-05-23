using Loja.Libraries.Email;
using Loja.Libraries.Filtro;
using Loja.Libraries.Lang;
using Loja.Libraries.Texto;
using Loja.Models.Constants;
using Loja.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Loja.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    [ColaboradorAutorizacao(ColaboradorTipoConstant.GERENTE)]
    public class ColaboradorController : Controller
    {
        private IColaboradorRepository _colaboradorRepository;
        private GerenciarEmail _gerenciarEmail;
        public ColaboradorController(IColaboradorRepository colaboradorRepository, GerenciarEmail gerenciarEmail)
        {
            _colaboradorRepository = colaboradorRepository;
            _gerenciarEmail = gerenciarEmail;
        }
        public IActionResult Index(int? pagina)
        {
            var colaboradores = _colaboradorRepository.ObterTodosColaboradores(pagina);
            return View(colaboradores);
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar([FromForm] Models.Colaborador colaborador)
        {
            ModelState.Remove("Senha"); // Não validar o campo de senha
            if (ModelState.IsValid)
            {
                var senha = KeyGenerator.GetUniqueKey(8);
                colaborador.Senha = senha;
                colaborador.Tipo = ColaboradorTipoConstant.COMUM;
                _colaboradorRepository.Cadastrar(colaborador);
                _gerenciarEmail.EnviarSenhaColaborador(colaborador);

                TempData["MSG_S"] = Mensagem.MSG_S001;
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        [ValidateHttpReferer]
        public IActionResult GerarSenha(int id)
        {
            var senha = KeyGenerator.GetUniqueKey(8);
            var colaborador = _colaboradorRepository.ObterColaborador(id);
            colaborador.Senha = senha;
            _colaboradorRepository.AtualizarSenha(colaborador);

            _gerenciarEmail.EnviarSenhaColaborador(colaborador);

            TempData["MSG_S"] = Mensagem.MSG_S004;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Atualizar(int id)
        {
            Models.Colaborador colaborador = _colaboradorRepository.ObterColaborador(id);
            return View(colaborador);
        }

        [HttpPost]
        public IActionResult Atualizar([FromForm] Models.Colaborador colaborador, int id)
        {
            ModelState.Remove("Senha");
            if (ModelState.IsValid)
            {
                _colaboradorRepository.Atualizar(colaborador);
                TempData["MSG_S"] = Mensagem.MSG_S002;
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        [ValidateHttpReferer]
        public IActionResult Excluir(int id)
        {
            _colaboradorRepository.Excluir(id);
            TempData["MSG_S"] = Mensagem.MSG_S003;
            return RedirectToAction("Index");
        }

    }
}
