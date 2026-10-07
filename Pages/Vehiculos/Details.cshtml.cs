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
    public class DetailsModel : PageModel
    {
        private readonly ProyectoDesarmado.Data.AppDbContext _context;

        public DetailsModel(ProyectoDesarmado.Data.AppDbContext context)
        {
            _context = context;
        }

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
    }
}
