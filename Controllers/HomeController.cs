using Loja.Libraries.Email;
using Loja.Libraries.Filtro;
using Loja.Libraries.Login;
using Loja.Models;
using Loja.Models.ViewModels;
using Loja.Repositories.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Loja.Controllers
{
    public class HomeController : Controller
    {
        private IClienteRepository _clienteRepository;
        private LoginCliente _loginCliente;
        private INewsletterRepository _newsletterRepository;
        private GerenciarEmail _gerenciarEmail;
        private IProdutoRepository _produtoRepository;
        public HomeController(IClienteRepository clienteRepository, INewsletterRepository newsletterRepository, LoginCliente loginCliente, GerenciarEmail gerenciarEmail, IProdutoRepository produtoRepository)
        {
            _clienteRepository = clienteRepository;
            _loginCliente = loginCliente;
            _newsletterRepository = newsletterRepository;
            _gerenciarEmail = gerenciarEmail;
            _produtoRepository = produtoRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index([FromForm]NewsletterEmail newsletter)
        {
            if (ModelState.IsValid)
            {
                _newsletterRepository.Cadastrar(newsletter);

                TempData["MSG_S"] = "<strong>E-mail cadastrado!</strong> <br/> Agora você vai receber promoções especiais no seu e-mail! Fique atento as novidades!";
                return RedirectToAction(nameof(Index));
            } else
            {
                return View();
            }
        }

        public IActionResult Categoria()
        {
            return View();
        }

        public IActionResult Contato()
        {
            return View();
        }

        public IActionResult ContatoAcao()
        {
            try
            {
                Contato contato = new Contato();
                contato.Nome = HttpContext.Request.Form["nome"];
                contato.Email = HttpContext.Request.Form["email"];
                contato.Texto = HttpContext.Request.Form["texto"];

                var ListaMensagens = new List<ValidationResult>();
                var contexto = new ValidationContext(contato);
                bool isValid = Validator.TryValidateObject(contato, contexto, ListaMensagens, true);

                if (isValid)
                {
                    _gerenciarEmail.EnviarContatoPorEmail(contato);
                    ViewData["MSG_S"] = "Mensagem de contato enviado com sucesso!";
                } else
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach(var texto in ListaMensagens)
                    {
                        stringBuilder.Append(texto.ErrorMessage + "<br>");
                    }

                    ViewData["MSG_E"] = stringBuilder.ToString();
                    ViewData["CONTATO"] = contato; // manter os dados na view quando houver erro
                }

            } catch (Exception e)
            {
                ViewData["MSG_E"] = "Ops! Tivemos um erro, tente novamente mais tarde!";
                // TODO - Implementar Log
            }
            return View("Contato");

            //return new ContentResult { Content = string.Format("Dados recebidos com sucesso!</br> Nome: {0} </br> E-mail: {1} </br> Texto: {2}", contato.Nome, contato.Email, contato.Texto),  ContentType = "text/html" };
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm]Cliente cliente)
        {
            var clienteDb = _clienteRepository.Login(cliente.Email, cliente.Senha);
            if (clienteDb != null)
            {
                _loginCliente.Login(clienteDb);
                return new RedirectResult(Url.Action(nameof(Painel)));
            } else
            {
                ViewData["MSG_E"] = "Usuário e senha inválidos!";
                return View();
            }
        }

        [HttpGet]
        [ClienteAutorizacao]
        public IActionResult Painel()
        {
            return new ContentResult() { Content = "Painel" };
        }

        public IActionResult CadastroCliente()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CadastroCliente([FromForm]Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _clienteRepository.Cadastrar(cliente);

                TempData["MSG_S"] = "<strong>Cadastro realizado com sucesso!</strong>";

                // TODO - Implementar redirecionamentos diferentes (painel, carrinho, etc)
                return RedirectToAction(nameof(CadastroCliente));
            } 
            return View();
        }

        public IActionResult CarrinhoCompras()
        {
            return View();
        }
    }
}

/*
 * TODO - Continuar em cap.16 aula 10
 */