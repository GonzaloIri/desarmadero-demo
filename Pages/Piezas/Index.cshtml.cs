using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProyectoDesarmado.Data;
using ProyectoDesarmado.Models;

namespace proyecto_desarmado.Pages.Piezas;

public class IndexModel : PageModel
{
    private readonly AppDbContext _context;

    public IndexModel(AppDbContext context)
    {
        _context = context;
    }

    public IList<Pieza> Pieza { get; set; } = new List<Pieza>();

    public string? Buscar { get; set; }
    public int PaginaActual { get; set; }
    public int TotalPaginas { get; set; }
    public int TamanoPagina { get; set; } = 25;
    public async Task OnGetAsync(string? buscar, int pagina = 1)    {
        Buscar = buscar;

        var consulta = _context.Piezas
            .Include(p => p.Vehiculo)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            string texto = buscar.Trim().ToUpperInvariant();

            consulta = consulta.Where(p =>
                p.Nombre.ToUpper().Contains(texto) ||
                (p.Categoria != null &&
                    p.Categoria.ToUpper().Contains(texto)) ||
                (p.Codigo != null &&
                    p.Codigo.ToUpper().Contains(texto)) ||
                (p.Ubicacion != null &&
                    p.Ubicacion.ToUpper().Contains(texto)) ||
                p.MarcaOrigen.ToUpper().Contains(texto) ||
                p.ModeloOrigen.ToUpper().Contains(texto) ||
                (p.Color != null && p.Color.ToUpper().Contains(texto)) ||
                (p.NumeroRudac != null && p.NumeroRudac.ToUpper().Contains(texto)) ||
                (p.NumeroPrecinto != null && p.NumeroPrecinto.ToUpper().Contains(texto)) ||
                (p.PatenteOrigen != null && p.PatenteOrigen.ToUpper().Contains(texto)) ||
                (p.Vehiculo != null && (
                    p.Vehiculo.Marca.ToUpper().Contains(texto) ||
                    p.Vehiculo.Modelo.ToUpper().Contains(texto) ||
                    (p.Vehiculo.Patente != null &&
                        p.Vehiculo.Patente.ToUpper().Contains(texto))))
            );
        }

        PaginaActual = Math.Max(pagina, 1);

var totalPiezas = await consulta.CountAsync();

TotalPaginas = (int)Math.Ceiling(
    totalPiezas / (double)TamanoPagina);

if (TotalPaginas > 0 && PaginaActual > TotalPaginas)
{
    PaginaActual = TotalPaginas;
}

Pieza = await consulta
    .OrderBy(p => p.Nombre)
    .ThenBy(p => p.Id)
    .Skip((PaginaActual - 1) * TamanoPagina)
    .Take(TamanoPagina)
    .ToListAsync();
    }
}