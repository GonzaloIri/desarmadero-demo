using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProyectoDesarmado.Data;
using ProyectoDesarmado.Models;

namespace proyecto_desarmado.Pages.Piezas
{
    public class DeleteModel : PageModel
    {
        private readonly ProyectoDesarmado.Data.AppDbContext _context;

        public DeleteModel(ProyectoDesarmado.Data.AppDbContext context)
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

            var pieza = await _context.Piezas.FirstOrDefaultAsync(m => m.Id == id);

            if (pieza is not null)
            {
                Pieza = pieza;

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

            var pieza = await _context.Piezas.FindAsync(id);
            if (pieza != null)
            {
                Pieza = pieza;
                _context.Piezas.Remove(Pieza);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
