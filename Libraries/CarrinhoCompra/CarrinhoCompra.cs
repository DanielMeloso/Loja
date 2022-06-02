using Loja.Models.ProdutoAgregador;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Loja.Libraries.CarrinhoCompra
{
    public class CarrinhoCompra
    {
        private Cookie.Cookie _cookie;
        private string key = "carrinho.compras";
        public CarrinhoCompra(Cookie.Cookie cookie)
        {
            _cookie = cookie;
        }

        public void Cadastrar(ProdutoItem item)
        {
            List<ProdutoItem> lista;
            if (_cookie.Existe(key))
            {
                lista = Consultar();
                var itemLocalizado = lista.FirstOrDefault(x => x.Id == item.Id);
                if (itemLocalizado == null)
                {
                    lista.Add(item);
                } else
                {
                    itemLocalizado.QuantidadeProdutoCarrinho = itemLocalizado.QuantidadeProdutoCarrinho + 1;
                }
                
            } else
            {
                lista = new List<ProdutoItem>();
                lista.Add(item);
            }

            Salvar(lista);
        }

        public void Atualizar(ProdutoItem item)
        {
            var lista = Consultar();
            var itemLocalizado = lista.SingleOrDefault(x => x.Id == item.Id);
            if (itemLocalizado != null)
            {
                itemLocalizado.QuantidadeProdutoCarrinho = item.QuantidadeProdutoCarrinho;
                Salvar(lista);
            }
        }

        public void Remover(ProdutoItem item)
        {
            var lista = Consultar();
            var itemLocalizado = lista.SingleOrDefault(x => x.Id == item.Id);
            if (itemLocalizado != null)
            {
                lista.Remove(itemLocalizado);
                Salvar(lista);
            }
        }

        public List<ProdutoItem> Consultar()
        {
            if (_cookie.Existe(key))
            {
                string valor = _cookie.Consultar(key);
                return JsonConvert.DeserializeObject<List<ProdutoItem>>(valor);
            } else
            {
                return new List<ProdutoItem>();
            }
        }

        public void Salvar(List<ProdutoItem> lista)
        {
            string valor = JsonConvert.SerializeObject(lista);
            _cookie.Cadastrar(key, valor);
        }

        public bool Existe(string key)
        {
            if (_cookie.Existe(key))
            {
                return false;
            }
            return true;
        }

        public void RemoverTodos()
        {
            _cookie.Remover(key);
        }
    }
}
