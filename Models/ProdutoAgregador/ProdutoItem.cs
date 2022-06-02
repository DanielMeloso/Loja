namespace Loja.Models.ProdutoAgregador
{
    public class ProdutoItem : Produto
    {
        // armazena a quantidade de produtos que o cliente pretende adquirir
        public int QuantidadeProdutoCarrinho { get; set; }
    }
}
