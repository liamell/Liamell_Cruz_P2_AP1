using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Liamell_Cruz_P2_AP1.Models;

public class Encuesta
{
    [Key]
    public int EncuestaId { get; set; }

    public DateTime Fecha { get; set; } = DateTime.Now;


    [Required(ErrorMessage = "LA asignatura debe ser obligatoria")]
    public string Asignatura { get; set; }

    [Required(ErrorMessage = "Por favor ingrese el monto")]
    [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "Solo se permiten números")]
    public double Monto { get; set; }


    [ForeignKey("EncuestaId")]
    public ICollection<EncuestaDetalle> encuestaDetalle { get; set; } = new List<EncuestaDetalle>();
}
