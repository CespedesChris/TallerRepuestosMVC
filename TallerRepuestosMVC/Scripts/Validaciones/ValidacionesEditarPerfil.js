document.addEventListener("DOMContentLoaded", function () {
    const formulario = document.getElementById("formEditarPerfil");

    if (!formulario) return; // Previene errores si el formulario no existe

    formulario.addEventListener("submit", function (e) {
        const nombre = document.getElementById("Nombre").value.trim();
        const correo = document.getElementById("Correo").value.trim();
        const contraseña = document.getElementById("Contraseña").value.trim();
        const rol = document.getElementById("Rol").value.trim();
        const error = document.getElementById("MensError");

        let mensaje = "";

        error.innerHTML = "";

        // Validación del nombre
        if (nombre === "") {
            mensaje += "<p>Debe ingresar el nombre.</p>";
        }

        // Validación del correo
        if (correo === "") {
            mensaje += "<p>Debe ingresar el correo.</p>";
        } else {
            const regexCorreo = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            if (!regexCorreo.test(correo)) {
                mensaje += "<p>Formato de correo inválido.</p>";
            }
        }

        // Validación de la contraseña
        if (contraseña === "") {
            mensaje += "<p>Debe ingresar la contraseña.</p>";
        }

        // Validación del rol
        if (rol === "") {
            mensaje += "<p>Debe seleccionar un rol.</p>";
        }

        if (mensaje !== "") {
            e.preventDefault();
            error.innerHTML = mensaje;
        }
    });
});