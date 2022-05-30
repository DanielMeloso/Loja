$(document).ready(function () {
    MoverScrollOrdenacao();
    MudarOrdenacao();
});

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

        var queryString = new URLSearchParams(window.location.search);
        if (queryString.has("pagina")) {
            pagina = queryString.get("pagina");
        }

        if (queryString.has("pesquisa")) {
            pesquisa = queryString.get("pesquisa");
        }

        var url = window.location.protocol + "//" + window.location.host + window.location.pathname;
        var urlComParemetros = url + "?pagina=" + pagina + "&pesquisa=" + pesquisa + "&ordenacao=" + ordenacao + "#posicao-produto";

        window.location.href = urlComParemetros;
    });
}