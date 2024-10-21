using motoRental.Models;

namespace motoRental.Interfaces
{
    public interface IMotoService
    {
        Task<Moto> AddMoto(Moto moto);
        Task<List<Moto>> GetMotos(string placa);
        Task<Moto> GetMotoById(string id);
        Task ChangeRegistration(string identificador, string novaPlaca);
        Task DeleteMoto(string id);
    }
}
