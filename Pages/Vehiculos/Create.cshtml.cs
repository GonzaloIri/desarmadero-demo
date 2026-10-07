using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoDesarmado.Data;
using ProyectoDesarmado.Models;
using Microsoft.EntityFrameworkCore;

namespace proyecto_desarmado.Pages.Vehiculos
{
    public class CreateModel : PageModel
    {
        private readonly ProyectoDesarmado.Data.AppDbContext _context;

        public CreateModel(ProyectoDesarmado.Data.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Vehiculo Vehiculo { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
{
    if (!ModelState.IsValid)
    {
        return Page();
    }

    if (!string.IsNullOrWhiteSpace(Vehiculo.Patente))
    {
        Vehiculo.Patente = Vehiculo.Patente
            .Replace(" ", "")
            .Replace("-", "")
            .Trim()
            .ToUpperInvariant();

        var patenteExiste = await _context.Vehiculos.AnyAsync(v =>
            v.Patente != null &&
            v.Patente.ToUpper() == Vehiculo.Patente);

        if (patenteExiste)
        {
            ModelState.AddModelError(
                "Vehiculo.Patente",
                "Ya existe un vehículo con esta patente.");
        }
    }

    if (!string.IsNullOrWhiteSpace(Vehiculo.NumeroCarpeta))
    {
        Vehiculo.NumeroCarpeta = Vehiculo.NumeroCarpeta.Trim();

        var carpetaExiste = await _context.Vehiculos.AnyAsync(v =>
            v.NumeroCarpeta == Vehiculo.NumeroCarpeta);

        if (carpetaExiste)
        {
            ModelState.AddModelError(
                "Vehiculo.NumeroCarpeta",
                "Ya existe un vehículo con este número de carpeta.");
        }
    }

    if (!ModelState.IsValid)
    {
        return Page();
    }

    _context.Vehiculos.Add(Vehiculo);
    await _context.SaveChangesAsync();

    return RedirectToPage("./Index");
}
    }
}
