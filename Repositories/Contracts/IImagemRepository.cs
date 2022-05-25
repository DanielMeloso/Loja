using Loja.Models;

namespace Loja.Repositories.Contracts
{
    public interface IImagemRepository
    {
        void Cadastrar(Imagem imagem);
        void Excluir(int id);
        Imagem ObterImagem(int id);
        void ExcluirImagensProduto(int produtoId);
    }
}
