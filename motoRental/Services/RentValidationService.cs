using Microsoft.EntityFrameworkCore;
using motoRental.Data;
using motoRental.DTO;
using motoRental.Interfaces;

namespace motoRental.Services
{
    public class RentValidationService : IRentValidationService
    {
        private readonly ApplicationDbContext _context;

        public RentValidationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ValidateRentRequestAsync(RentRequestDTO rentRequest)
        {
            var moto = await _context.Motos.FirstOrDefaultAsync(m => m.Identificador == rentRequest.Moto_Id);
            if (moto == null)
            {
                throw new KeyNotFoundException("Moto não encontrada.");
            }

            var entregador = await _context.DeliveryGuys.FirstOrDefaultAsync(e => e.Identificador == rentRequest.Entregador_Id);
            if (entregador == null)
            {
                throw new KeyNotFoundException("Entregador não encontrado.");
            }

            if (!entregador.Tipo_Cnh.Contains("A"))
            {
                throw new InvalidOperationException("Entregador não habilitado na categoria A.");
            }

            if (rentRequest.Data_Inicio <= rentRequest.Data_Prevista_Termino)
            {
                throw new ArgumentException("A data de término prevista deve ser posterior à data de início.");
            }
        }
    }

}
