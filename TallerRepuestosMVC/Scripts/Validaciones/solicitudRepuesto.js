document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("form");
    const cantidad = document.querySelector("#CantidadSolicitada");

    form.addEventListener("submit", function (e) {
        if (!cantidad.value || cantidad.value <= 0) {
            e.preventDefault();
            alert("Ingrese una cantidad válida.");
        }
    });
});