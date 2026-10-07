using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProyectoDesarmado.Data;
using ProyectoDesarmado.Models;

namespace proyecto_desarmado.Pages.Vehiculos
{
    public class DeleteModel : PageModel
    {
        private readonly ProyectoDesarmado.Data.AppDbContext _context;

        public DeleteModel(ProyectoDesarmado.Data.AppDbContext context)
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

            if (vehiculo is not null)
            {
                Vehiculo = vehiculo;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo != null)
            {
                Vehiculo = vehiculo;
                _context.Vehiculos.Remove(Vehiculo);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
