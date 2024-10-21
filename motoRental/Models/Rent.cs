using System.ComponentModel.DataAnnotations;

namespace motoRental.Models
{
    public class Rent
    {
        [Key]
        public string Identificador { get; set; } // Ex: "locacao123"

        [Required]
        public string Entregador_Id { get; set; } // Ex: "entregador123"

        [Required]
        public string Moto_Id { get; set; } // Ex: "moto123"

        [Required]
        public DateTime Data_Inicio { get; set; } // Ex: "2024-01-01T00:00:00Z"

        [Required]
        public DateTime Data_Prevista_Termino { get; set; } // Ex: "2024-01-07T23:59:59Z"

        public DateTime? Data_Termino { get; set; } // Ex: "2024-01-07T23:59:59Z" (null até devolução)

        public int Plano { get; set; } // Ex: 7 (dias)

        public decimal ValorDiaria { get; set; } // Valor diário da locação, definido pelo plano
    }
}
