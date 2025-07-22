    document.addEventListener("DOMContentLoaded", function () {
        const formulario = document.getElementById("loginForm");

        formulario.addEventListener("submit", function (e) {
            const correo = document.getElementById("Correo").value.trim();
            const contraseña = document.getElementById("Contraseña").value.trim();
            const error = document.getElementById("MensError");
            let mensaje = "";



            // Acá se limpian los mensajes anteriores
            error.innerHTML = "";

            //Se valida el correo
            if (correo === "") {
                mensaje += "<p>Debe ingresar el correo.</p>";
            } else {
                const regexCorreo = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
                if (!regexCorreo.test(correo)) {
                    mensaje += "<p>Formato de correo inválido.</p>";
                }
            }


            // Se valida la contraseña
            if (contraseña === "") {
                mensaje += "Debe ingresar la contraseña.\n";
            }

            if (mensaje !== "") {
                e.preventDefault();
                error.innerHTML = mensaje;
            }
        });
    });