using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using motoRental.Data;
using motoRental.Interfaces;
using motoRental.Models;
using motoRental.Services;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
namespace motoRental.Tests.Services
{
    public class DeliveryGuyServiceTests
    {
        private readonly DeliveryGuyService _deliveryGuyService;
        private readonly Mock<IStorageService> _storageServiceMock;
        private readonly Mock<ILogger<DeliveryGuyService>> _loggerMock;
        private readonly ApplicationDbContext _context;

        public DeliveryGuyServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _storageServiceMock = new Mock<IStorageService>();
            _loggerMock = new Mock<ILogger<DeliveryGuyService>>();
            _deliveryGuyService = new DeliveryGuyService(_context, _storageServiceMock.Object, _loggerMock.Object);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task AddDeliveryGuy_ShouldAddDeliveryGuy_WhenCnpjAndCnhAreUnique()
        {
            // Arrange
            var entregador = new DeliveryGuy
            {
                Identificador = Guid.NewGuid().ToString(),
                Nome = "testeGuy",
                Cnpj = "123456782",
                Numero_Cnh = "CNH123411",
                Tipo_Cnh= "A",
                Imagem_Cnh = "imagem_base64"
            };

            // Configura o mock do StorageService para salvar a imagem e retornar uma URL
            _storageServiceMock.Setup(x => x.SaveImage(It.IsAny<string>()))
                               .ReturnsAsync("image_url");

            // Act
            var result = await _deliveryGuyService.AddDeliveryGuy(entregador);

            // Assert
            var addedDeliveryGuy = await _context.DeliveryGuys.FirstOrDefaultAsync(e => e.Cnpj == "123456782");

            Assert.NotNull(addedDeliveryGuy);
            Assert.Equal("123456782", addedDeliveryGuy.Cnpj);
            Assert.Equal("image_url", addedDeliveryGuy.Imagem_Cnh);
        }

        [Fact]
        public async Task AddDeliveryGuy_ShouldThrowException_WhenCnpjAlreadyExists()
        {
            // Arrange
            var existingDeliveryGuy = new DeliveryGuy
            {
                Identificador = "123",
                Nome = "testeGuy",
                Cnpj = "123456789",
                Numero_Cnh = "CNH123456",
                Tipo_Cnh = "A",
                Imagem_Cnh = "imagem_base64"
            };

            await _context.DeliveryGuys.AddAsync(existingDeliveryGuy);
            await _context.SaveChangesAsync();

            var newDeliveryGuy = new DeliveryGuy
            {
                Identificador = "123",
                Nome = "testeGuy",
                Cnpj = "123456789",
                Numero_Cnh = "CNH123457",
                Tipo_Cnh = "A",
                Imagem_Cnh = "imagem_base64"
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _deliveryGuyService.AddDeliveryGuy(newDeliveryGuy));
            Assert.Equal("CNPJ já registrado.", exception.Message);
        }

        [Fact]
        public async Task AddDeliveryGuy_ShouldThrowException_WhenCnhAlreadyExists()
        {
            // Arrange
            var existingDeliveryGuy = new DeliveryGuy
            {
                Identificador = Guid.NewGuid().ToString(),
                Nome = "testeGuy",
                Cnpj = "987654321",
                Numero_Cnh = "CNH123456",
                Tipo_Cnh = "A",
                Imagem_Cnh = "imagem_base64"
            };

            await _context.DeliveryGuys.AddAsync(existingDeliveryGuy);
            await _context.SaveChangesAsync();

            var newDeliveryGuy = new DeliveryGuy
            {
                Identificador = Guid.NewGuid().ToString(),
                Nome = "testeGuy",
                Cnpj = "222222222",
                Numero_Cnh = "CNH123456",
                Tipo_Cnh = "A",
                Imagem_Cnh = "imagem_base64"
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _deliveryGuyService.AddDeliveryGuy(newDeliveryGuy));
            Assert.Equal("CNH já registrada.", exception.Message);
        }

        [Fact]
        public async Task UpdatePhotoCnh_ShouldThrowException_WhenDeliveryGuyNotFound()
        {
            // Arrange
            var nonExistentId = "invalid123";
            var newImageCnh = "new_image_base64";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _deliveryGuyService.UpdatePhotoCnh(nonExistentId, newImageCnh));
            Assert.Equal("Entregador não encontrado.", exception.Message);
        }
    }
}