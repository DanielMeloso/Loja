using AutoMapper;
using Loja.Models.ProdutoAgregador;

namespace Loja.Libraries.Automapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Produto, ProdutoItem>();
        }
    }
}
