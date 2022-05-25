$(document).ready(function () {
    $(".btn-danger").click(function (e) {
        var resultado = confirm("Deseja realizar essa operação?");

        if (resultado == false) {
            e.preventDefault();
        }
    });

    $('.dinheiro').mask('000.000.000.000.000,00', { reverse: true });

    AjaxUploadImagemProduto();
});

function AjaxUploadImagemProduto() {
    $(".img-upload").click(function () {
        $(this).parent().parent().find(".input-file").click();
    });

    $(".btn-imagem-excluir").click(function () {
        var campoHidden = $(this).parent().find("input[name=imagem]");
        var imagem = $(this).parent().find(".img-upload");
        var btnExcluir = $(this).parent().find(".btn-imagem-excluir");
        var inputFile = $(this).parent().find(".input-file");

        $.ajax({
            type: "GET",
            url: "/Colaborador/Imagem/Deletar?caminho=" + campoHidden.val(),
            error: function () {
                
            },
            success: function (data) {
                imagem.attr("src", "/img/sem-foto.jpeg");
                btnExcluir.addClass("btn-ocultar");
                campoHidden.val("");
                inputFile.val("");
            }

        });
    });

    $(".input-file").change(function () {
        // Formulário de dados via JS
        var formulario = new FormData();
        var binario = $(this)[0].files[0];
        formulario.append("file", binario);

        var campoHidden = $(this).parent().find("input[name=imagem]");
        var imagem = $(this).parent().find(".img-upload");
        var btnExcluir = $(this).parent().find(".btn-imagem-excluir");

        // Apresentação da imagem de loading
        imagem.attr("src", "/img/loading.gif");
        imagem.addClass("thumb");


        // Requisição Ajax enviando o formulário criado
        $.ajax({
            type: "POST",
            url: "/Colaborador/Imagem/Armazenar",
            data: formulario,
            contentType: false,
            processData: false, // não realizar a validação dos dados enviados
            error: function () {
                alert("Erro no envio do arquivo!");
                imagem.attr("src", "/img/sem-foto.jpeg");
                imagem.removeClass("thumb");
            },
            success: function (data) {
                var caminho = data.caminho;
                imagem.attr("src", caminho);
                campoHidden.val(caminho);
                btnExcluir.removeClass("btn-ocultar");
                imagem.removeClass("thumb");
            }

        });

    });
}