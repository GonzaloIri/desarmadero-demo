using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoDesarmado.Data;
using ProyectoDesarmado.Models;

namespace proyecto_desarmado.Pages.Piezas
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
        ViewData["VehiculoId"] = new SelectList(_context.Vehiculos, "Id", "Descripcion");
            return Page();
        }

        [BindProperty]
        public Pieza Pieza { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Piezas.Add(Pieza);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
