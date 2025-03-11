using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Liamell_Cruz_P2_AP1.Models;

public class EncuestaDetalle
{
    [Key]
    public int DetalleId { get; set; }

    [ForeignKey("Encuestas")]
    public int EncuestaId { get; set; }

    public Encuesta? Encuesta { get; set; }

    [ForeignKey("Ciudades")]
    public int CiudadId { get; set; }
    public Ciudades? Ciudades { get; set; }

    [Required(ErrorMessage = "Por favor ingrese el monto")]
    [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "Solo se permiten números enteros o decimales")]
    public double Monto { get; set; }
}
