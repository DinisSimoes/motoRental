using motoRental.Models;

namespace motoRental.Interfaces
{
    public interface IMotoEventPublisher
    {
        void PublishMotoAddedEvent(Moto moto);
    }
}
