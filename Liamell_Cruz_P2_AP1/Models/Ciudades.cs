using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Liamell_Cruz_P2_AP1.Models;

public class Ciudades
{
    [Key]
    
        public int CiudadId { get; set; }

        public string Nombre { get; set; }
        public double Monto { get; set; }

    



}


