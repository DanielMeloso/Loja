using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using X.PagedList;

namespace Loja.Models.ViewModels
{
    public class ProdutoListagemViewModel
    {
        public IPagedList<Produto> listaProduto { get; set; }
        public List<SelectListItem> ordenacao
        {
            get
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem("A-Z", "A"),
                    new SelectListItem("Menor Preço", "ME"),
                    new SelectListItem("Maior Preço", "MA")
                };
            }
            private set { }
        }
    }
}
