using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClosedXML.Excel;
using ProyectoDesarmado.Data;
using ProyectoDesarmado.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
namespace proyecto_desarmado.Pages.Piezas;

public class ImportarModel : PageModel

{
        private readonly AppDbContext _context;

    public ImportarModel(AppDbContext context)
    {
        _context = context;
    }
    [BindProperty]
public IFormFile? ArchivoExcel { get; set; }

[BindProperty]
public bool ConfirmarReemplazo { get; set; }

public string? Mensaje { get; set; }

    public void OnGet()
    {
    }

    public IActionResult OnPost(string? accion)
    {
        if (ArchivoExcel == null || ArchivoExcel.Length == 0)
        {
            Mensaje = "Seleccioná un archivo Excel.";
            return Page();
        }

        if (!string.Equals(
            System.IO.Path.GetExtension(ArchivoExcel.FileName),
            ".xlsx",
            StringComparison.OrdinalIgnoreCase))
        {
            Mensaje = "El archivo debe tener formato .xlsx.";
            return Page();
        }

        try
{using var stream = new MemoryStream();
ArchivoExcel.CopyTo(stream);

stream.Position = 0;

var huellaArchivo =
    Convert.ToHexString(SHA256.HashData(stream));

stream.Position = 0;

if (_context.ImportacionesStock
    .Any(i => i.HuellaArchivo == huellaArchivo))
{
    Mensaje = "Este archivo ya fue importado anteriormente.";
    return Page();
}

using var libro = new XLWorkbook(stream);
var hojasPermitidas = new[] { "CHAPA", "PERIFERICOS" };

foreach (var nombreHoja in hojasPermitidas)
{
    if (!libro.TryGetWorksheet(nombreHoja, out _))
    {
        Mensaje = $"No se encontró la hoja {nombreHoja}.";
        return Page();
    }
}
    var piezasParaImportar = new List<Pieza>();
    var conRudac = 0;
    var sinRudac = 0;
    var repetidos = 0;
    var detalleRepetidos = new List<string>();

    var rudacsEncontrados =
        new HashSet<string>(StringComparer.OrdinalIgnoreCase);

    foreach (var nombreHoja in hojasPermitidas)
    {
        var hoja = libro.Worksheet(nombreHoja);

        // CHAPA: columna D. PERIFERICOS: columna F.
        var columnaRudac = nombreHoja == "CHAPA" ? 4 : 6;

        foreach (var fila in hoja.RowsUsed())
        {
            // Las primeras dos filas contienen títulos.
            if (fila.RowNumber() <= 2)
            {
                continue;
            }

            var rudac = fila.Cell(columnaRudac)
                .GetFormattedString()
                .Trim();

            if (string.IsNullOrWhiteSpace(rudac))
            {
                sinRudac++;
                continue;
            }

            conRudac++;
            var pieza = new Pieza
{
    Nombre = fila.Cell(1).GetFormattedString().Trim(),
    MarcaOrigen = fila.Cell(2).GetFormattedString().Trim(),
    ModeloOrigen = fila.Cell(3).GetFormattedString().Trim(),
    NumeroRudac = rudac,
    Categoria = nombreHoja == "CHAPA"
        ? "Chaperío"
        : "Periféricos",
    Estado = "Disponible",
    VehiculoId = null
};

if (nombreHoja == "CHAPA")
{
    if (fila.Cell(5).TryGetValue<int>(out var anio))
    {
        pieza.AnioOrigen = anio;
    }

    if (fila.Cell(6).TryGetValue<decimal>(out var precio))
    {
        pieza.Precio = precio;
    }

    pieza.Color = fila.Cell(7).GetFormattedString().Trim();
    pieza.Ubicacion = fila.Cell(10).GetFormattedString().Trim();
}
else
{
    pieza.PatenteOrigen =
        fila.Cell(4).GetFormattedString().Trim();

    pieza.Observaciones =
        fila.Cell(8).GetFormattedString().Trim();
}

piezasParaImportar.Add(pieza);
            if (!rudacsEncontrados.Add(rudac))
{
    repetidos++;

    detalleRepetidos.Add(
        $"RUDAC {rudac}, hoja {nombreHoja}, " +
        $"fila {fila.RowNumber()}: " +
        $"{fila.Cell(1).GetFormattedString()} " +
        $"{fila.Cell(2).GetFormattedString()} " +
        $"{fila.Cell(3).GetFormattedString()}");
}
        }
    }
    if (accion == "guardar")
{
    if (!ConfirmarReemplazo)
    {
        Mensaje = "Marcá la confirmación para reemplazar las piezas de prueba.";
        return Page();
    }

    if (piezasParaImportar.Count == 0)
    {
        Mensaje = "No se encontraron piezas con RUDAC. No se modificó nada.";
        return Page();
    }

    var hayDatosIncompletos = piezasParaImportar.Any(p =>
        string.IsNullOrWhiteSpace(p.Nombre) ||
        string.IsNullOrWhiteSpace(p.MarcaOrigen) ||
        string.IsNullOrWhiteSpace(p.ModeloOrigen));

    if (hayDatosIncompletos)
    {
        Mensaje = "Hay piezas sin nombre, marca o modelo. Revisá el Excel.";
        return Page();
    }

    using var transaccion = _context.Database.BeginTransaction();

    if (_context.ImportacionesStock.Any())
    {
        Mensaje =
            "Ya se realizó una importación. La carga de archivos " +
            "actualizados todavía no está habilitada.";
        return Page();
    }

    var piezasDePrueba = _context.Piezas.ToList();

    _context.Piezas.RemoveRange(piezasDePrueba);
    _context.Piezas.AddRange(piezasParaImportar);

    _context.ImportacionesStock.Add(new ImportacionStock
    {
        NombreArchivo = System.IO.Path.GetFileName(ArchivoExcel.FileName),
        HuellaArchivo = huellaArchivo,
        FechaUtc = DateTime.UtcNow,
        CantidadPiezas = piezasParaImportar.Count
    });

    _context.SaveChanges();
    transaccion.Commit();

    Mensaje =
        $"Importación completada: {piezasParaImportar.Count} piezas guardadas. " +
        "Se reemplazaron las piezas de prueba y se conservaron los vehículos.";

    return Page();
}

        Mensaje =
        $"Revisión terminada: {conRudac} filas con RUDAC, " +
        $"{sinRudac} sin RUDAC y {repetidos} repeticiones de RUDAC. " +
        "Todavía no se guardó ni se borró ninguna pieza.";

    if (detalleRepetidos.Count > 0)
    {
        Mensaje += " Revisar: " + string.Join("; ", detalleRepetidos);
    }
}
catch (Exception)
{
    Mensaje =
        Mensaje =
    "No se pudo completar la operación. " +
    "Revisá el archivo y la conexión con la base de datos.";
}

return Page();
    }
}