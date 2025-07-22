document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("formRepuesto");
    const mensajeError = document.getElementById("mensajeError");

    form.addEventListener("submit", function (e) {
        let valido = true;
        let errores = [];

        const nombre = document.getElementById("Nombre").value.trim();
        const cantidad = document.getElementById("Cantidad").value.trim();
        const precio = document.getElementById("Precio").value.trim();

        if (nombre === "") {
            valido = false;
            errores.push("El nombre es obligatorio.");
        }

        if (cantidad === "" || isNaN(cantidad) || parseInt(cantidad) <= 0) {
            valido = false;
            errores.push("La cantidad debe ser un número mayor a 0.");
        }

        if (precio === "" || isNaN(precio) || parseFloat(precio) <= 0) {
            valido = false;
            errores.push("El precio debe ser un número mayor a 0.");
        }

        if (!valido) {
            e.preventDefault();
            mostrarErrores(errores);
        } else {
            mensajeError.innerHTML = ""; // limpia errores si todo está bien
        }
    });

    function mostrarErrores(errores) {
        let lista = "<ul>";
        errores.forEach(error => {
            lista += `<li>${error}</li>`;
        });
        lista += "</ul>";
        mensajeError.innerHTML = lista;
        mensajeError.style.display = "block";
    }
});