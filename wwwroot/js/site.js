(() => {
    const button = document.getElementById("menu-toggle");
    const label = document.getElementById("menu-label");
    const menu = document.getElementById("menu-principal");
    if (!button || !label || !menu) return;
    function update() {
        const hidden = document.documentElement.classList.contains("menu-hidden");
        button.setAttribute("aria-expanded", String(!hidden));
        label.textContent = hidden ? "Mostrar menú" : "Ocultar menú";
    }
    button.addEventListener("click", () => {
        const hidden = document.documentElement.classList.toggle("menu-hidden");
        try { localStorage.setItem("desarmadero.menu.oculto", String(hidden)); } catch (_) {}
        update();
    });
    update();
})();
