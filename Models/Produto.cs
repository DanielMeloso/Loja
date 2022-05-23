using Loja.Libraries.Lang;
using System.ComponentModel.DataAnnotations;

namespace Loja.Models
{
    public class Produto
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E001")]
        [MinLength(4, ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E002")]
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }

    }
}
