using System.ComponentModel.DataAnnotations;

namespace motoRental.Models
{
    public class Moto
    {
        [Key]
        public string Identificador { get; set; } // Ex: "moto123"

        [Required]
        public int Ano { get; set; } // Ex: 2020

        [Required]
        [MaxLength(100)]
        public string Modelo { get; set; } // Ex: "Mottu Sport"

        [Required]
        [MaxLength(10)]
        public string Placa { get; set; } // Ex: "CDX-0101"
    }
}
