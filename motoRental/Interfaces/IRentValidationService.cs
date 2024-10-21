using motoRental.DTO;

namespace motoRental.Interfaces
{
    public interface IRentValidationService
    {
        Task ValidateRentRequestAsync(RentRequestDTO rentRequest);
    }
}
