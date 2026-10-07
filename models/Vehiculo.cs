using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ProyectoDesarmado.Models;

public class Vehiculo
{
    public int Id { get; set; }

    [Display(Name = "Carpeta")]
    public string? NumeroCarpeta { get; set; }

    [Display(Name = "Tipo de baja")]
    public string? TipoBaja { get; set; }

    [Required]
    public string Marca { get; set; } = string.Empty;

    [Required]
    public string Modelo { get; set; } = string.Empty;

    public int Anio { get; set; }

    public string? Patente { get; set; }


    [Display(Name = "Número de motor")]
    public string? NumeroMotor { get; set; }
    
    public string? NumeroChasis { get; set; }

    [DataType(DataType.Date)]
    public DateTime FechaIngreso { get; set; } = DateTime.Today;

    public string? Observaciones { get; set; }
    [Display(Name = "Número de siniestro")]
public string? NumeroSiniestro { get; set; }

[Display(Name = "Color")]
public string? Color { get; set; }

[Display(Name = "Compañía aseguradora")]
public string? CompaniaSeguro { get; set; }

[Display(Name = "Asegurado")]
public string? Asegurado { get; set; }

[Display(Name = "Número de trámite")]
public string? NumeroTramite { get; set; }

[Required]
[Display(Name = "Estado")]
public string Estado { get; set; } = "Ingresado";

[Display(Name = "Tiene papeles")]
public bool TienePapeles { get; set; }

[Display(Name = "Destino")]
public string? Destino { get; set; }

    [NotMapped]
public string Descripcion =>
    $"{Marca} {Modelo} - {Patente ?? "Sin patente"}";
}