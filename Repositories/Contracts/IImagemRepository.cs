using Loja.Models;
using System.Collections.Generic;

namespace Loja.Repositories.Contracts
{
    public interface IImagemRepository
    {
        void Cadastrar(Imagem imagem);
        void CadastrarImagens(List<Imagem> listaImagens, int produtoId);
        void Excluir(int id);
        Imagem ObterImagem(int id);
        void ExcluirImagensProduto(int produtoId);
    }
}
