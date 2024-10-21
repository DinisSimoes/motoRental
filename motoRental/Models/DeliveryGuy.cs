using System.ComponentModel.DataAnnotations;

namespace motoRental.Models
{
    public class DeliveryGuy
    {
        [Key]
        public string Identificador { get; set; } // Ex: "entregador123"

        [Required]
        [MaxLength(150)]
        public string Nome { get; set; } // Ex: "João da Silva"

        [Required]
        [MaxLength(14)]
        public string Cnpj { get; set; } // Ex: "12345678901234", CNPJ único

        [Required]
        public DateTime DataNascimento { get; set; } // Ex: "1990-01-01T00:00:00Z"

        [Required]
        [MaxLength(11)]
        public string Numero_Cnh { get; set; } // Ex: "12345678900", CNH única

        [Required]
        [MaxLength(3)]
        public string Tipo_Cnh { get; set; } // Ex: "A", "B", "A+B"

        public string Imagem_Cnh { get; set; } // Ex: "base64string" (pode ser URL após armazenamento)
    }
}
