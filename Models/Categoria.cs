using Loja.Libraries.Lang;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Loja.Models
{
    public class Categoria
    {
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E001")]
        [MinLength(4, ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E002")]
        // TODO - Criar validação - Nome Categoria único no banco de dados
        public string Nome { get; set; }
        /*
         * www.lojavirtual.com.br/categoria/1
         * www.lojavirtual.com.br/categoria/informatica (url amigável)
         * 
         * Nome: Telefone sem Fio
         * Slug: telefone-sem-fio
         */
        // TODO - Criar validação de slug unico (geração automatica de slug).
        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E001")]
        [MinLength(4, ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MSG_E002")]
        public string Slug { get; set; }

        /*
         * Auto-relacionamento
         */
        [Display(Name = "Categoria Pai")]
        public int? CategoriaPaiId { get; set; }

        [ForeignKey("CategoriaPaiId")]
        public virtual Categoria CategoriaPai { get; set; }


    }
}
