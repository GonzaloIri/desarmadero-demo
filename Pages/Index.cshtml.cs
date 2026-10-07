using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProyectoDesarmado.Data;

namespace proyecto_desarmado.Pages;

public class IndexModel : PageModel
{
    private readonly AppDbContext _context;

    public IndexModel(AppDbContext context)
    {
        _context = context;
    }

    public int CantidadVehiculos { get; set; }
    public int CantidadPiezas { get; set; }
    public int PiezasDisponibles { get; set; }

    public async Task OnGetAsync()
    {
        CantidadVehiculos = await _context.Vehiculos.CountAsync();
        CantidadPiezas = await _context.Piezas.CountAsync();

        PiezasDisponibles = await _context.Piezas
            .CountAsync(p => p.Estado == "Disponible");
            PiezasReservadas = await _context.Piezas
    .CountAsync(p => p.Estado == "Reservada");

PiezasVendidas = await _context.Piezas
    .CountAsync(p => p.Estado == "Vendida");
    }
    public int PiezasReservadas { get; set; }
public int PiezasVendidas { get; set; }
}