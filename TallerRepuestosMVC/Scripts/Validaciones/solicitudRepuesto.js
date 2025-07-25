document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("form");
    const cantidad = document.querySelector("#CantidadSolicitada");
    const repuesto = document.querySelector("#RepuestoId");


    form.addEventListener("submit", function (e) {
        if (!repuesto.value || repuesto.value === "0") {
            e.preventDefault();
            alert("Debe seleccionar un repuesto.");
            return;
        }
        if (!cantidad.value || cantidad.value <= 0) {
            e.preventDefault();
            alert("Ingrese una cantidad válida.");
        }
    });
});