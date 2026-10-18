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

// Mostrar los campos con errores aunque su sección esté plegada.
document.querySelectorAll('.inventory-form').forEach(form => {
    form.addEventListener('invalid', event => {
        const section = event.target.closest('details');
        if (section) section.open = true;
    }, true);
    if (window.jQuery) {
        window.jQuery(form).on('invalid-form.validate', (_event, validator) => {
            validator.errorList.forEach(error => {
                const section = error.element.closest('details');
                if (section) section.open = true;
            });
        });
    }
});

// La fila abre la ficha; el enlace del nombre también permite usar el teclado.
document.querySelectorAll('.inventory-row').forEach(row => {
    row.addEventListener('click', event => {
        if (event.button !== 0 || event.ctrlKey || event.metaKey || event.shiftKey || event.altKey) return;
        if (event.target.closest('a, button, input, select, textarea, label')) return;
        if (window.getSelection()?.toString()) return;
        window.location.assign(row.dataset.detailsUrl);
    });
});
