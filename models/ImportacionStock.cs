using System.ComponentModel.DataAnnotations;

namespace ProyectoDesarmado.Models;

public class ImportacionStock
{
    public int Id { get; set; }

    [Required]
    public string NombreArchivo { get; set; } = string.Empty;

    [Required]
    [MaxLength(64)]
    public string HuellaArchivo { get; set; } = string.Empty;

    public DateTime FechaUtc { get; set; } = DateTime.UtcNow;

    public int CantidadPiezas { get; set; }
}