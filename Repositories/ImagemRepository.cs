using Loja.Database;
using Loja.Models;
using Loja.Repositories.Contracts;
using System.Linq;

namespace Loja.Repositories
{
    public class ImagemRepository : IImagemRepository
    {
        private LojaContext _banco;

        public ImagemRepository(LojaContext banco)
        {
            _banco = banco;
        }
        public void Cadastrar(Imagem imagem)
        {
            _banco.Add(imagem);
            _banco.SaveChanges();
        }

        public void Excluir(int id)
        {
            var imagem = ObterImagem(id);
            _banco.Remove(imagem);
            _banco.SaveChanges();
        }
        public Imagem ObterImagem(int id)
        {
            return _banco.Imagens.Find(id);
        }

        public void ExcluirImagensProduto(int produtoId)
        {
            var imagens = _banco.Imagens.Where(x => x.ProdutoId == produtoId).ToList();
            foreach(var imagem in imagens)
            {
                _banco.Remove(imagem); 
            }
            _banco.SaveChanges();
        }

    }
}