using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ProyectoDesarmado.Models;

public class Pieza
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Nombre de la pieza")]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Categoría")]
    public string Categoria { get; set; } = string.Empty;

    [Display(Name = "Código interno")]
    public string? Codigo { get; set; }

    [Required]
    [Display(Name = "Estado")]
    public string Estado { get; set; } = "Disponible";

    [Display(Name = "Precio")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Precio { get; set; }

    [Display(Name = "Ubicación")]
    public string? Ubicacion { get; set; }

    [Display(Name = "Observaciones")]
    public string? Observaciones { get; set; }

    [Required]
    [Display(Name = "Marca de origen")]
    public string MarcaOrigen { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Modelo de origen")]
    public string ModeloOrigen { get; set; } = string.Empty;

    [Display(Name = "Número RUDAC")]
    public string? NumeroRudac { get; set; }

    [Display(Name = "Número de precinto")]
    public string? NumeroPrecinto { get; set; }

    [Display(Name = "Año")]
    public int? AnioOrigen { get; set; }

    [Display(Name = "Color")]
    public string? Color { get; set; }

    [Display(Name = "Patente de origen")]
    public string? PatenteOrigen { get; set; }

    [Display(Name = "Compatibilidades")]
    public string? Compatibilidades { get; set; }
    [Display(Name = "Vehículo registrado")]
    public int? VehiculoId { get; set; }

    [ValidateNever]
    public Vehiculo? Vehiculo { get; set; }
}
