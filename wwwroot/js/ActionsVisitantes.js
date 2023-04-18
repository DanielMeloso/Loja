$(document).ready(function () {
    MoverScrollOrdenacao();
    MudarOrdenacao();
    MudarImagemPrincipalProduto();
    MudarQuantidadeProdutoCarrinho();
    MascaraCep();
    AjaxCalcularFrete();
});

function AjaxCalcularFrete() {
    $(".btn-calcular-frete").click(function () {
        var cep = $(".cep").val().replace("-", "");

        $.ajax({
            type: "GET",
            url: "/CarrinhoCompra/CalcularFrete?cepDestino=" + cep,
            error: function (data) {
                console.info(data);
                
            },
            success: function (data) {
                console.info(data);
            }

        });
    });
}

function MascaraCep() {
    $(".cep").mask("00000-000");
}

function MudarQuantidadeProdutoCarrinho() {
    $("#order .btn-primary").click(function () {
        if ($(this).hasClass("diminuir")) {
            OrquestradorDeAcoesProduto("diminuir", $(this));
        }
        if ($(this).hasClass("aumentar")) {
            OrquestradorDeAcoesProduto("aumentar", $(this));
        }
    });
}

function OrquestradorDeAcoesProduto(operacao, botao) {
    OcultarMensagemErro();

    var pai = botao.parent();

    var produtoId = pai.find(".inputProdutoId").val();
    var qtdeEstoque = parseInt(pai.find(".inputQuantidadeEstoque").val());
    var valorUnitario = parseFloat(pai.find(".inputValorUnitario").val().replace(",","."));

    var campoQuantidadeProdutoCarrinho = pai.find(".inputQuantidadeProdutoCarrinho");
    var qtdeProdutoCarrinhoAntiga = parseInt(campoQuantidadeProdutoCarrinho.val());

    var campoValor = botao.parent().parent().parent().find(".price");

    var produto = new ProdutoQuantidadeValor(produtoId, qtdeEstoque, valorUnitario, qtdeProdutoCarrinhoAntiga, 0, campoQuantidadeProdutoCarrinho, campoValor);

    AlteracoesVisuaisProdutoCarrinho(produto, operacao);
}

function AlteracoesVisuaisProdutoCarrinho(produto, operacao) {
    if (operacao == "aumentar") {
        //if (produto.qtdeEstoque == produto.qtdeProdutoCarrinhoAntiga) {
        //    alert("Não possuímos estoque suficiente");
        //} else
        {
            produto.qtdeProdutoCarrinhoNova = produto.qtdeProdutoCarrinhoAntiga + 1;

            produto.campoQuantidadeProdutoCarrinho.val(produto.qtdeProdutoCarrinhoNova);
            AtualizarQtdeValor(produto);
            AjaxComunicarAlteracaoQuantidadeProduto(produto);
        }
    } else if (operacao == "diminuir") {
        //if (produto.qtdeProdutoCarrinhoAntiga == 1) {
        //    alert("Caso não deseje esse produto, clique no botão remover")
        //} else
        {
            produto.qtdeProdutoCarrinhoNova = produto.qtdeProdutoCarrinhoAntiga - 1;
            AtualizarQtdeValor(produto);

            AjaxComunicarAlteracaoQuantidadeProduto(produto);
        }
    }
}

function MostrarMensagemErro(mensagem) {
    $(".alert-danger").css("display", "block");
    $(".alert-danger").text(mensagem);
}

function OcultarMensagemErro() {
    $(".alert-danger").css("display", "none");
}

function AjaxComunicarAlteracaoQuantidadeProduto(produto) {
    $.ajax({
        type: "GET",
        url: "/CarrinhoCompra/AlterarQuantidade?id=" + produto.produtoId + "&quantidade=" + produto.qtdeProdutoCarrinhoNova,
        error: function (data) {
            MostrarMensagemErro(data.responseJSON.mensagem);
            // Rollback
            produto.qtdeProdutoCarrinhoNova = produto.qtdeProdutoCarrinhoAntiga;
            AtualizarQtdeValor(produto);
        },
        success: function (data) {
            imagem.attr("src", "/img/sem-foto.jpeg");
            btnExcluir.addClass("btn-ocultar");
            campoHidden.val("");
            inputFile.val("");
        }

    });
}

function AtualizarQtdeValor(produto) {
    produto.campoQuantidadeProdutoCarrinho.val(produto.qtdeProdutoCarrinhoNova);

    var resultado = (produto.valorUnitario * produto.qtdeProdutoCarrinhoNova);
    produto.campoValor.text(numberToReal(resultado));

    AtualizarSubTotal();
}

function AtualizarSubTotal() {
    var subTotal = 0;
    var tagsComPrice = $(".price");

    tagsComPrice.each(function () {
        var valorReais = parseFloat($(this).text().replace("R$", "").replace(".", "").replace(" ", "").replace(",", "."));
        subTotal += valorReais;
    });

    $(".subtotal").text(numberToReal(subTotal));
}

function MudarImagemPrincipalProduto() {
    $(".img-small-wrap img").click(function () {
        var caminho = $(this).attr("src");
        $(".img-big-wrap img").attr("src", caminho);
        $(".img-big-wrap a").attr("href", caminho);
    });
}

function MoverScrollOrdenacao() {
    if (window.location.hash.length > 0) {
        var hash = window.location.hash;
        if (hash == "#posicao-produto") {
            window.scrollBy(0,473);
        }
    }
}

function MudarOrdenacao() {
    $("#ordenacao").change(function () {
        var pagina = 1;
        var pesquisa = "";
        var ordenacao = $(this).val();
        var fragmento = "#posicao-produto";

        var queryString = new URLSearchParams(window.location.search);
        if (queryString.has("pagina")) {
            pagina = queryString.get("pagina");
        }

        if (queryString.has("pesquisa")) {
            pesquisa = queryString.get("pesquisa");
        }

        if ($("#breadcrumb").length > 0) {
            fragmento = "";
        }

        var url = window.location.protocol + "//" + window.location.host + window.location.pathname;
        var urlComParemetros = url + "?pagina=" + pagina + "&pesquisa=" + pesquisa + "&ordenacao=" + ordenacao + fragmento;

        window.location.href = urlComParemetros;
    });
}

function numberToReal(numero) {
    var numero = numero.toFixed(2).split('.');
    numero[0] = "R$ " + numero[0].split(/(?=(?:...)*$)/).join('.');
    return numero.join(',');
}


/*------------------------------------------------- CLASSES -------------------------------------------------------*/
class ProdutoQuantidadeValor {
    constructor(produtoId, qtdeEstoque, valorUnitario, qtdeProdutoCarrinhoAntiga, qtdeProdutoCarrinhoNova, campoQuantidadeProdutoCarrinho, campoValor) {
        this.produtoId = produtoId;
        this.qtdeEstoque = qtdeEstoque;
        this.valorUnitario = valorUnitario;

        this.qtdeProdutoCarrinhoAntiga = qtdeProdutoCarrinhoAntiga;
        this.qtdeProdutoCarrinhoNova = qtdeProdutoCarrinhoNova;

        this.campoQuantidadeProdutoCarrinho = campoQuantidadeProdutoCarrinho;
        this.campoValor = campoValor;
    }
}