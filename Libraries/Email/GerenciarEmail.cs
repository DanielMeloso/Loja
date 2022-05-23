using Loja.Models;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Loja.Libraries.Email
{
    public class GerenciarEmail
    {
        private SmtpClient _smtpClient;
        private IConfiguration _configuration;
        public GerenciarEmail(SmtpClient smtpClient, IConfiguration configuration)
        {
            _smtpClient = smtpClient;
            _configuration = configuration;
        }

        public void EnviarContatoPorEmail(Contato contato)
        {
            string corpoMsg = string.Format("<h2> Contato - Loja Daniel </h2>"
                + "<b>Nome:</b> {0} <br>"
                + "<b>E-mail:</b> {1} <br>"
                + "<b>Texto: </b> {2} <br><br>"
                + "E-mail enviado automaticamente do site Loja Daniel.",
                contato.Nome, contato.Email, contato.Texto
                );

            MailMessage mensagem = new MailMessage();
            mensagem.From = new MailAddress(_configuration.GetValue<string>("Email:Username"));
            mensagem.To.Add(contato.Email);
            mensagem.Subject = "Contato - Loja Daniel - E-mail " + contato.Email;
            mensagem.Body = corpoMsg;
            mensagem.IsBodyHtml = true;

            _smtpClient.Send(mensagem);
        }

        public void EnviarSenhaColaborador(Colaborador colaborador)
        {
            string corpoMsg = string.Format("<h2> Senha - Loja Daniel </h2>"
                + "<b>Nome:</b> {0} <br><br>"
                + "Faça o login no sistema utilizando seu e-mail e senha:<br>"
                + "<b>E-mail:</b> {1}<br>"
                + "<b>Senha:</b> {2}<br><br>"
                + "E-mail enviado automaticamente do site Loja Daniel.",
                colaborador.Nome, colaborador.Email, colaborador.Senha
                );

            MailMessage mensagem = new MailMessage();
            mensagem.From = new MailAddress(_configuration.GetValue<string>("Email:Username"));
            mensagem.To.Add(colaborador.Email);
            mensagem.Subject = "Colaborador - Loja Daniel - Senha do Colaborador " + colaborador.Nome;
            mensagem.Body = corpoMsg;
            mensagem.IsBodyHtml = true;

            _smtpClient.Send(mensagem);
        }
    }
}
