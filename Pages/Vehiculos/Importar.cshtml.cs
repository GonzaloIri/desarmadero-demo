using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProyectoDesarmado.Data;
using ProyectoDesarmado.Models;

namespace proyecto_desarmado.Pages.Vehiculos;

public class ImportarModel : PageModel
{
    private readonly AppDbContext _context;

    public ImportarModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public IFormFile? ArchivoExcel { get; set; }

    public string? Mensaje { get; set; }

    public string? Error { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ArchivoExcel == null || ArchivoExcel.Length == 0)
        {
            Error = "Seleccioná un archivo Excel.";
            return Page();
        }

        if (Path.GetExtension(ArchivoExcel.FileName).ToLowerInvariant() != ".xlsx")
        {
            Error = "El archivo debe tener formato .xlsx.";
            return Page();
        }

        try
        {
            using var stream = ArchivoExcel.OpenReadStream();
            using var libro = new XLWorkbook(stream);

            if (!libro.TryGetWorksheet("Hoja1", out var hoja))
            {
                Error = "No se encontró la hoja llamada Hoja1.";
                return Page();
            }

            var vehiculosExistentes = await _context.Vehiculos
    .Where(v => v.Patente != null)
    .ToListAsync();

            var vehiculosPorPatente = vehiculosExistentes
                .GroupBy(v => NormalizarPatente(v.Patente!))
                .ToDictionary(
                    grupo => grupo.Key,
                    grupo => grupo.First(),
                    StringComparer.OrdinalIgnoreCase);

            var importados = 0;
            var actualizados = 0;
            var duplicados = 0;
            var omitidos = 0;

            foreach (var fila in hoja.RowsUsed().Skip(1))
            {
                var patente = Limpiar(fila.Cell(4).GetFormattedString());
                var marca = Limpiar(fila.Cell(5).GetFormattedString());
                var modelo = Limpiar(fila.Cell(6).GetFormattedString());
                var numeroCarpeta = Limpiar(fila.Cell(1).GetFormattedString());
                var tipoBaja = Limpiar(fila.Cell(11).GetFormattedString());

                if (string.IsNullOrWhiteSpace(patente) ||
                    string.IsNullOrWhiteSpace(marca) ||
                    string.IsNullOrWhiteSpace(modelo))
                {
                    omitidos++;
                    continue;
                }

                var patenteNormalizada = NormalizarPatente(patente);

                if (vehiculosPorPatente.TryGetValue(
        patenteNormalizada, out var existente))
                {
                    var seActualizo = false;

                    if (string.IsNullOrWhiteSpace(existente.NumeroCarpeta) &&
                        !string.IsNullOrWhiteSpace(numeroCarpeta))
                    {
                        existente.NumeroCarpeta = numeroCarpeta;
                        seActualizo = true;
                    }

                    if (string.IsNullOrWhiteSpace(existente.TipoBaja) &&
                        !string.IsNullOrWhiteSpace(tipoBaja))
                    {
                        existente.TipoBaja = tipoBaja.ToUpperInvariant();
                        seActualizo = true;
                    }

                    if (seActualizo)
                    {
                        actualizados++;
                    }
                    else
                    {
                        duplicados++;
                    }

                    continue;
                }

                var observacion1 = Limpiar(fila.Cell(24).GetFormattedString());
                var observacion2 = Limpiar(fila.Cell(33).GetFormattedString());

                var vehiculo = new Vehiculo
                {
                    NumeroCarpeta = numeroCarpeta,
                    TipoBaja = tipoBaja?.ToUpperInvariant(),
                    Patente = patenteNormalizada,
                    Marca = marca,
                    Modelo = modelo,
                    Color = Limpiar(fila.Cell(8).GetFormattedString()),
                    NumeroMotor = Limpiar(fila.Cell(9).GetFormattedString()),
                    NumeroChasis = Limpiar(fila.Cell(10).GetFormattedString()),
                    CompaniaSeguro = Limpiar(fila.Cell(12).GetFormattedString()),
                    Estado = NormalizarEstado(
                        fila.Cell(15).GetFormattedString()),
                    NumeroSiniestro = Limpiar(
                        fila.Cell(25).GetFormattedString()),
                    Asegurado = Limpiar(
                        fila.Cell(27).GetFormattedString()),
                    Destino = Limpiar(
                        fila.Cell(23).GetFormattedString()),
                    Observaciones = UnirObservaciones(
                        observacion1, observacion2),
                    FechaIngreso = ObtenerFecha(fila.Cell(2))
                };

                _context.Vehiculos.Add(vehiculo);
                vehiculosPorPatente[patenteNormalizada] = vehiculo;
                importados++;
            }

            await _context.SaveChangesAsync();

            Mensaje = $"Importación terminada: {importados} vehículos agregados, " +
                $"{actualizados} vehículos actualizados, " +
                $"{duplicados} duplicados y {omitidos} filas omitidas.";

            return Page();
        }
        catch (Exception ex)
        {
            Error = $"No se pudo importar el archivo: {ex.Message}";
            return Page();
        }
    }

    private static string NormalizarPatente(string patente)
    {
        return patente
            .Replace(" ", "")
            .Replace("-", "")
            .Trim()
            .ToUpperInvariant();
    }

    private static string? Limpiar(string? valor)
    {
        var texto = valor?.Trim();

        if (string.IsNullOrWhiteSpace(texto) ||
            texto.Equals("X", StringComparison.OrdinalIgnoreCase) ||
            texto.Equals("#N/A", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        return texto;
    }

    private static string NormalizarEstado(string? estado)
    {
        return (estado?.Trim().ToUpperInvariant() ?? "") switch
        {
            "EN TRAMITE" => "En trámite",
            "FINALIZADO" => "Finalizado",
            "CANCELADO" => "Cancelado",
            "SIN NOVEDADES" => "Sin novedades",
            "AUN NO INGRESO AL REGISTRO" => "Aún no ingresó al registro",
            "JUICIO" => "Juicio",
            "04D PARA HACER" => "04D para hacer",
            _ => "Ingresado"
        };
    }

    private static DateTime ObtenerFecha(IXLCell celda)
    {
        if (celda.TryGetValue<DateTime>(out var fecha))
        {
            return fecha;
        }

        return DateTime.Today;
    }

    private static string? UnirObservaciones(
        string? primera, string? segunda)
    {
        var observaciones = new[] { primera, segunda }
            .Where(x => !string.IsNullOrWhiteSpace(x));

        var resultado = string.Join(" | ", observaciones);

        return string.IsNullOrWhiteSpace(resultado)
            ? null
            : resultado;
    }
}
