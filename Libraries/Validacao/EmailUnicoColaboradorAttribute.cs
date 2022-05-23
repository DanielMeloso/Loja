using Loja.Models;
using Loja.Repositories.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Loja.Libraries.Validacao
{
    public class EmailUnicoColaboradorAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var email = (value as string).Trim();

            var _colaboradorRepository = (IColaboradorRepository)validationContext.GetService(typeof(IColaboradorRepository));
            var colaboradores = _colaboradorRepository.ObterColaboradorPorEmail(email);

            var objColaborador = (Colaborador)validationContext.ObjectInstance;

            if (colaboradores.Count > 1)
            {
                return new ValidationResult("E-mail já cadastrado!");
            }

            if (colaboradores.Count == 1 && objColaborador.Id != colaboradores[0].Id)
            {
                return new ValidationResult("E-mail já cadastrado!");
            }

            return ValidationResult.Success;
        }
    }
}
