using motoRental.Interfaces;

namespace motoRental.Services
{
    public class StorageService : IStorageService
    {
        private readonly string _storagePath;

        public StorageService()
        {
            _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath); // Cria o diretório, se não existir
            }
        }

        public async Task<string> SaveImage(string imagemBase64)
        {
            if (string.IsNullOrEmpty(imagemBase64))
            {
                throw new ArgumentNullException(nameof(imagemBase64), "A imagem Base64 não pode ser nula ou vazia.");
            }

            var imageBytes = Convert.FromBase64String(imagemBase64);
            var fileName = $"{Guid.NewGuid()}.jpg"; // Nome único para a imagem
            var filePath = Path.Combine(_storagePath, fileName);

            await File.WriteAllBytesAsync(filePath, imageBytes); // Salva a imagem localmente

            // Retorna o caminho relativo para ser utilizado na aplicação (como URL)
            return $"/images/{fileName}";
        }

        public void DeleteImage(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath)) return;

            var filePath = Path.Combine(_storagePath, Path.GetFileName(imagePath));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
