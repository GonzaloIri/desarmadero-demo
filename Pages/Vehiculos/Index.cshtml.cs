using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProyectoDesarmado.Data;
using ProyectoDesarmado.Models;

namespace proyecto_desarmado.Pages.Vehiculos;

public class IndexModel : PageModel
{
    private readonly AppDbContext _context;

    public IndexModel(AppDbContext context)
    {
        _context = context;
    }

    public IList<Vehiculo> Vehiculo { get; set; } = new List<Vehiculo>();

    public string? Buscar { get; set; }
    public string? EstadoSeleccionado { get; set; }

    public int PaginaActual { get; set; }
    public int TotalPaginas { get; set; }
    public int TamanoPagina { get; set; } = 25;

    public async Task OnGetAsync(
    string? buscar,
    string? estado,
    int pagina = 1)
    {
        Buscar = buscar;
        EstadoSeleccionado = estado;

        var consulta = _context.Vehiculos.AsQueryable();

        if (!string.IsNullOrWhiteSpace(buscar))
{
    string texto = buscar.Trim();

    if (texto.All(char.IsDigit))
    {
        consulta = consulta.Where(v =>
            v.NumeroCarpeta == texto);
    }
   else
{
    var textoNormalizado = texto.ToUpper();

    consulta = consulta.Where(v =>
        v.Marca.ToUpper().Contains(textoNormalizado) ||
        v.Modelo.ToUpper().Contains(textoNormalizado) ||
        (v.Patente != null &&
            v.Patente.ToUpper().Contains(textoNormalizado)) ||
        (v.NumeroChasis != null &&
            v.NumeroChasis.ToUpper().Contains(textoNormalizado)) ||
        (v.NumeroMotor != null &&
            v.NumeroMotor.ToUpper().Contains(textoNormalizado)) ||
        (v.NumeroSiniestro != null &&
            v.NumeroSiniestro.ToUpper().Contains(textoNormalizado)) ||
        (v.NumeroTramite != null &&
            v.NumeroTramite.ToUpper().Contains(textoNormalizado)) ||
        (v.CompaniaSeguro != null &&
            v.CompaniaSeguro.ToUpper().Contains(textoNormalizado)) ||
        (v.Asegurado != null &&
            v.Asegurado.ToUpper().Contains(textoNormalizado)) ||
        v.Estado.ToUpper().Contains(textoNormalizado));
}
  }      
        if (!string.IsNullOrWhiteSpace(estado))
{
    consulta = consulta.Where(v => v.Estado == estado);
}
      PaginaActual = Math.Max(pagina, 1);

var totalVehiculos = await consulta.CountAsync();

TotalPaginas = (int)Math.Ceiling(
    totalVehiculos / (double)TamanoPagina);

if (TotalPaginas > 0 && PaginaActual > TotalPaginas)
{
    PaginaActual = TotalPaginas;
}

Vehiculo = await consulta
    .OrderBy(v => v.Marca)
    .ThenBy(v => v.Modelo)
    .Skip((PaginaActual - 1) * TamanoPagina)
    .Take(TamanoPagina)
    .ToListAsync();
    }
}