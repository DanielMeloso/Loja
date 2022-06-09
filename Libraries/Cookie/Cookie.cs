using Loja.Libraries.Seguranca;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace Loja.Libraries.Cookie
{
    public class Cookie
    {
        private IHttpContextAccessor _context;
        private IConfiguration _configuration;
        public Cookie(IHttpContextAccessor context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public void Cadastrar(string key, string valor)
        {
            var options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(7);
            options.IsEssential = true;

            var valorCrypt = StringCipher.Encrypt(valor, _configuration.GetValue<string>("KeyCrypt"));

            _context.HttpContext.Response.Cookies.Append(key, valorCrypt, options);
        }

        public void Atualizar(string key, string valor)
        {
            if (Existe(key))
            {
                Remover(key);
            }
            Cadastrar(key, valor);
        }

        public void Remover(string key)
        {
            _context.HttpContext.Response.Cookies.Delete(key);
        }

        public string Consultar(string key)
        {
            var valorCrypt = _context.HttpContext.Request.Cookies[key];
            var valor = StringCipher.Decrypt(valorCrypt, _configuration.GetValue<string>("KeyCrypt"));
            return valor;
        }

        public bool Existe(string key)
        {
            if (_context.HttpContext.Request.Cookies[key] == null)
            {
                return false;
            }
            return true;
        }

        public void RemoverTodos()
        {
            var listaCookies = _context.HttpContext.Request.Cookies.ToList();
            foreach(var cookie in listaCookies)
            {
                Remover(cookie.Key);
            }
        }
    }
}
