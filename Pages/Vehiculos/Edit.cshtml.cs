using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoDesarmado.Data;
using ProyectoDesarmado.Models;

namespace proyecto_desarmado.Pages.Vehiculos
{
    public class EditModel : PageModel
    {
        private readonly ProyectoDesarmado.Data.AppDbContext _context;

        public EditModel(ProyectoDesarmado.Data.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Vehiculo Vehiculo { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehiculo = await _context.Vehiculos.FirstOrDefaultAsync(m => m.Id == id);
            if (vehiculo == null)
            {
                return NotFound();
            }
            Vehiculo = vehiculo;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
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
                    v.Id != Vehiculo.Id &&
                    v.Patente != null &&
                    v.Patente.ToUpper() == Vehiculo.Patente);

                if (patenteExiste)
                {
                    ModelState.AddModelError(
                        "Vehiculo.Patente",
                        "Ya existe otro vehículo con esta patente.");
                }
            }

            if (!string.IsNullOrWhiteSpace(Vehiculo.NumeroCarpeta))
            {
                Vehiculo.NumeroCarpeta = Vehiculo.NumeroCarpeta.Trim();

                var carpetaExiste = await _context.Vehiculos.AnyAsync(v =>
                    v.Id != Vehiculo.Id &&
                    v.NumeroCarpeta == Vehiculo.NumeroCarpeta);

                if (carpetaExiste)
                {
                    ModelState.AddModelError(
                        "Vehiculo.NumeroCarpeta",
                        "Ya existe otro vehículo con este número de carpeta.");
                }
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Vehiculo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehiculoExists(Vehiculo.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToPage("./Index");
        }

        private bool VehiculoExists(int id)
        {
            return _context.Vehiculos.Any(e => e.Id == id);
        }
    }
}
