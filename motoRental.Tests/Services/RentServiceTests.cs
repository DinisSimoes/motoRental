using Microsoft.EntityFrameworkCore;
using Moq;
using motoRental.Data;
using motoRental.DTO;
using motoRental.Interfaces;
using motoRental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace motoRental.Tests.Services
{
    public class RentServiceTests
    {
        private readonly ApplicationDbContext _context;
        private readonly RentService _rentService;
        private readonly Mock<IRentValidationService> _validationServiceMock;

        public RentServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _validationServiceMock = new Mock<IRentValidationService>();
            _rentService = new RentService(_context, _validationServiceMock.Object);
        }

        [Fact]
        public async Task CreateNewRentAsync_ShouldCreateRent_WhenValidRentRequest()
        {
            // Arrange
            var rentRequest = new RentRequestDTO
            {
                Entregador_Id = "1",
                Moto_Id = "1",
                Data_Inicio = DateTime.Now,
                Data_Prevista_Termino = DateTime.Now.AddDays(7),
                Plano = 7
            };

            _validationServiceMock.Setup(v => v.ValidateRentRequestAsync(It.IsAny<RentRequestDTO>()))
                                  .Returns(Task.CompletedTask);

            // Act
            var rent = await _rentService.CreateNewRentAsync(rentRequest);

            // Assert
            Assert.NotNull(rent);
            Assert.Equal(rentRequest.Moto_Id, rent.Moto_Id);
            Assert.Equal(rentRequest.Entregador_Id, rent.Entregador_Id);
            Assert.Equal(30m, rent.ValorDiaria);  // Valor esperado com plano de 7 dias
        }

        [Fact]
        public async Task CreateNewRentAsync_ShouldCallValidationService_WhenCalled()
        {
            // Arrange
            var rentRequest = new RentRequestDTO
            {
                Entregador_Id = "1",
                Moto_Id = "1",
                Data_Inicio = DateTime.Now,
                Data_Prevista_Termino = DateTime.Now.AddDays(7),
                Plano = 7
            };

            // Act
            await _rentService.CreateNewRentAsync(rentRequest);

            // Assert
            _validationServiceMock.Verify(v => v.ValidateRentRequestAsync(rentRequest), Times.Once);
        }

        [Fact]
        public async Task GetRentByIdAsync_ShouldThrowKeyNotFoundException_WhenRentNotFound()
        {
            // Arrange
            var rentId = "nonexistent-id";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _rentService.GetRentByIdAsync(rentId));
            Assert.Equal("Locação não encontrada.", exception.Message);
        }

        [Fact]
        public async Task GetRentByIdAsync_ShouldReturnRent_WhenRentExists()
        {
            // Arrange
            var rent = new Rent
            {
                Identificador = "1",
                Entregador_Id = "1",
                Moto_Id = "1",
                Data_Inicio = DateTime.Now,
                Data_Prevista_Termino = DateTime.Now.AddDays(7)
            };

            _context.Rents.Add(rent);
            await _context.SaveChangesAsync();

            // Act
            var result = await _rentService.GetRentByIdAsync("1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(rent.Identificador, result.Identificador);
        }

        [Theory]
        [InlineData(7, 7, 5, 2, 162)]   // Retorno antes do previsto (com penalidade)
        [InlineData(7, 7, 7, 0, 210)]   // Devolução no prazo
        [InlineData(7, 7, 9, 2, 320)]   // Devolução após o prazo (com multa)
        public void CalculateRentValue_ShouldReturnCorrectRentValue(int plano, int diasPrevistos, int diasDevolvidos, int diasMulta, decimal valorEsperado)
        {
            // Arrange
            var rent = new Rent
            {
                Data_Inicio = DateTime.Now,
                Data_Prevista_Termino = DateTime.Now.AddDays(diasPrevistos),
                ValorDiaria = 30m,
                Plano = plano
            };

            var dataDevolucao = rent.Data_Inicio.AddDays(diasDevolvidos);

            // Act
            var valorTotal = _rentService.CalculateRentValue(rent, rent.Data_Prevista_Termino, dataDevolucao);

            // Assert
            Assert.Equal(valorEsperado, valorTotal);
        }
    }
}
