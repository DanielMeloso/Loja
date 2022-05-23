using Loja.Libraries.Login;
using Loja.Models;
using Loja.Models.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Loja.Libraries.Filtro
{
    public class ColaboradorAutorizacaoAttribute : Attribute, IAuthorizationFilter
    {
        LoginColaborador _loginColaborador;
        private string _tipoAutorizado;

        public ColaboradorAutorizacaoAttribute(string tipoAutorizado = ColaboradorTipoConstant.COMUM)
        {
            _tipoAutorizado = tipoAutorizado;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _loginColaborador = (LoginColaborador)context.HttpContext.RequestServices.GetService(typeof(LoginColaborador));
            Colaborador colaborador = _loginColaborador.GetColaborador();

            if (colaborador == null)
            {
                context.Result = new RedirectToActionResult(nameof(Login), "Home", null);
            } else
            {
                if (colaborador.Tipo == ColaboradorTipoConstant.COMUM && _tipoAutorizado == ColaboradorTipoConstant.GERENTE)
                {
                    context.Result = new ForbidResult();
                }
            }
        }
    }
}
