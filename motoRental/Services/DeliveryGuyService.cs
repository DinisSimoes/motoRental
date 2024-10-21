using Microsoft.EntityFrameworkCore;
using motoRental.Data;
using motoRental.Interfaces;
using motoRental.Models;

namespace motoRental.Services
{
    public class DeliveryGuyService : IDeliveryGuyService
    {
        private readonly ApplicationDbContext _context;
        private readonly IStorageService _storageService;
        private readonly ILogger<DeliveryGuyService> _logger;

        public DeliveryGuyService(ApplicationDbContext context, IStorageService storageService, ILogger<DeliveryGuyService> logger)
        {
            _context = context;
            _storageService = storageService;
            _logger = logger;
        }

        public async Task<DeliveryGuy> AddDeliveryGuy(DeliveryGuy entregador)
        {
            if (await _context.DeliveryGuys.AnyAsync(e => e.Cnpj == entregador.Cnpj))
                throw new InvalidOperationException("CNPJ já registrado.");

            if (await _context.DeliveryGuys.AnyAsync(e => e.Numero_Cnh == entregador.Numero_Cnh))
                throw new InvalidOperationException("CNH já registrada.");

            if (!string.IsNullOrEmpty(entregador.Imagem_Cnh))
            {
                try
                {
                    var urlImagem = await _storageService.SaveImage(entregador.Imagem_Cnh);
                    entregador.Imagem_Cnh = urlImagem;
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Erro ao salvar a imagem da CNH.");
                }
            }

            _context.DeliveryGuys.Add(entregador);
            await _context.SaveChangesAsync();

            return entregador;
        }

        public async Task UpdatePhotoCnh(string identificador, string imagemCnh)
        {
            var entregador = await _context.DeliveryGuys.FirstOrDefaultAsync(e => e.Identificador == identificador);
            if (entregador == null)
                throw new KeyNotFoundException("Entregador não encontrado.");

            // Remove a imagem existente, se houver
            _storageService.DeleteImage(entregador.Imagem_Cnh);

            // Lógica para salvar a nova imagem da CNH
            var urlImagem = await _storageService.SaveImage(imagemCnh);

            entregador.Imagem_Cnh = urlImagem;
            await _context.SaveChangesAsync();
        }
    }

}
