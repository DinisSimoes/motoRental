using motoRental.Models;

namespace motoRental.Interfaces
{
    public interface IDeliveryGuyService
    {
        Task<DeliveryGuy> AddDeliveryGuy(DeliveryGuy entregador);
        Task UpdatePhotoCnh(string identificador, string imagemCnh);
    }
}
