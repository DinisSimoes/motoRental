namespace motoRental.Interfaces
{
    public interface IStorageService
    {
        Task<string> SaveImage(string imagemBase64);
        void DeleteImage(string imagemBase64);
    }
}
