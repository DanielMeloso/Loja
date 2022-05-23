$(document).ready(function () {
    $(".btn-danger").click(function (e) {
        var resultado = confirm("Deseja excluir esse registro?");

        if (resultado == false) {
            e.preventDefault();
        }
    });
});