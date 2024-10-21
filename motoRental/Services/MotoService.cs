using Microsoft.EntityFrameworkCore;
using motoRental.Data;
using motoRental.Interfaces;
using motoRental.Logging;
using motoRental.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace motoRental.Services
{
    public class MotoService : IMotoService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MotoService> _logger;
        private readonly IMotoEventPublisher _motoEventPublisher;

        public MotoService(ApplicationDbContext context, ILogger<MotoService> logger, IMotoEventPublisher motoEventPublisher)
        {
            _context = context;
            _logger = logger;
            _motoEventPublisher = motoEventPublisher;
        }

        public async Task<Moto> AddMoto(Moto moto)
        {
            if (await _context.Motos.AnyAsync(m => m.Placa == moto.Placa))
            {
                throw new InvalidOperationException(LogMessages.writelog(LogMessages.MotoAlreadyExists, moto.Placa));
            }

            _context.Motos.Add(moto);
            await _context.SaveChangesAsync();

            _motoEventPublisher.PublishMotoAddedEvent(moto);

            return moto;

        }

        public async Task<List<Moto>> GetMotos(string placa)
        {
            return string.IsNullOrEmpty(placa)
                ? await _context.Motos.ToListAsync()
                : await _context.Motos.Where(m => m.Placa == placa).ToListAsync();
        }

        public async Task<Moto> GetMotoById(string id)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto == null)
                throw new KeyNotFoundException("Moto não encontrada.");

            return moto;
        }

        public async Task ChangeRegistration(string identificador, string novaPlaca)
        {
            var moto = await _context.Motos.FirstOrDefaultAsync(m => m.Identificador == identificador);
            if (moto == null)
                throw new KeyNotFoundException("Moto não encontrada.");

            if (await _context.Motos.AnyAsync(m => m.Placa == novaPlaca))
                throw new InvalidOperationException("Placa já registrada.");

            moto.Placa = novaPlaca;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMoto(string id)
        {
            var moto = await _context.Motos.FirstOrDefaultAsync(m => m.Identificador == id);
            if (moto == null)
                throw new InvalidOperationException("Moto não encontrada.");

            if (await _context.Rents.AnyAsync(l => l.Moto_Id == moto.Identificador))
                throw new InvalidOperationException("Moto possui registros de locação.");

            _context.Motos.Remove(moto);
            await _context.SaveChangesAsync();
        }
    }
}
