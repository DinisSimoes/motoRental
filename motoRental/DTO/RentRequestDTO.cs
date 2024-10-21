using System.ComponentModel.DataAnnotations;

namespace motoRental.DTO
{
    public class RentRequestDTO
    {
        [Required]
        public string Entregador_Id { get; set; }

        [Required]
        public string Moto_Id { get; set; }

        [Required]
        public DateTime Data_Inicio { get; set; }

        [Required]
        public DateTime Data_Prevista_Termino { get; set; }

        [Required]
        public DateTime Data_Termino { get; set; }

        [Required]
        public int Plano { get; set; }
    }

}
