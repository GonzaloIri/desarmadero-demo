// Aplicar la preferencia antes de dibujar la página evita que el menú parpadee.
(() => {
    try {
        if (localStorage.getItem("desarmadero.menu.oculto") === "true") {
            document.documentElement.classList.add("menu-hidden");
        }
    } catch (_) { /* El menú funciona aunque el navegador bloquee el almacenamiento. */ }
})();
