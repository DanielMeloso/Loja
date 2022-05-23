using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Loja.Libraries.Filtro
{
    public class ValidateHttpRefererAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Executado após passar pelo controllador

            string referer = context.HttpContext.Request.Headers["Referer"].ToString();

            if (string.IsNullOrEmpty(referer))
            {
                // mesmo que esteja logado, se digitar a URL no navegador não irá executar a função. Necessário clicar para executar
                context.Result = new ContentResult() { Content = "Acesso Negado!" };
            } else
            {
                Uri uri = new Uri(referer);

                string hostReferer = uri.Host;
                string hostServidor = context.HttpContext.Request.Host.Host;

                if (hostReferer != hostServidor)
                {
                    context.Result = new ContentResult() { Content = "Acesso Negado!" };
                }
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Executado antes passar pelo controlador
            
        }
    }
}
