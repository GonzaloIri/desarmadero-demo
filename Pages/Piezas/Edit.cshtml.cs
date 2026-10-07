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


namespace proyecto_desarmado.Pages.Piezas
{
    public class EditModel : PageModel
    {
        private readonly ProyectoDesarmado.Data.AppDbContext _context;

        public EditModel(ProyectoDesarmado.Data.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Pieza Pieza { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pieza =  await _context.Piezas.FirstOrDefaultAsync(m => m.Id == id);
            if (pieza == null)
            {
                return NotFound();
            }
            Pieza = pieza;
           ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "Id", "Descripcion");
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

            _context.Attach(Pieza).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PiezaExists(Pieza.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool PiezaExists(int id)
        {
            return _context.Piezas.Any(e => e.Id == id);
        }
    }
}
