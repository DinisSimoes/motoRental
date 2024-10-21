using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using motoRental.Data;
using motoRental.DTO;
using motoRental.Interfaces;
using motoRental.Models;

public class RentService
{
    private readonly ApplicationDbContext _context;
    private readonly IRentValidationService _validationService;

    public RentService(ApplicationDbContext context, IRentValidationService validationService)
    {
        _context = context;
        _validationService = validationService;
    }

    public async Task<Rent> CreateNewRentAsync(RentRequestDTO rentRequest)
    {
        await _validationService.ValidateRentRequestAsync(rentRequest);

        var novaLocacao = new Rent
        {
            Identificador = Guid.NewGuid().ToString(),
            Entregador_Id = rentRequest.Entregador_Id,
            Moto_Id = rentRequest.Moto_Id,
            Data_Inicio = rentRequest.Data_Inicio,
            Data_Prevista_Termino = rentRequest.Data_Prevista_Termino,
            Plano = rentRequest.Plano,
            ValorDiaria = GetDiaryValue(rentRequest.Plano),
            Data_Termino = rentRequest.Data_Termino
        };

        _context.Rents.Add(novaLocacao);
        await _context.SaveChangesAsync();

        return novaLocacao;
    }

    public async Task<Rent> GetRentByIdAsync(string id)
    {
        var locacao = await _context.Rents.FirstOrDefaultAsync(l => l.Identificador == id);
        if (locacao == null) throw new KeyNotFoundException("Locação não encontrada.");

        return locacao;
    }

    public decimal CalculateRentValue(Rent locacao, DateTime dataPrevistaTermino, DateTime dataDevolucao)
    {
        TimeSpan diasLocados = dataDevolucao - locacao.Data_Inicio;
        decimal valorTotal = diasLocados.Days * locacao.ValorDiaria;

        if (dataDevolucao < dataPrevistaTermino)
        {
            TimeSpan diasRestantes = dataPrevistaTermino - dataDevolucao;
            valorTotal += diasRestantes.Days * locacao.ValorDiaria * GetPercentagePenalty(locacao.Plano);
        }
        else if (dataDevolucao > dataPrevistaTermino)
        {
            TimeSpan diasAdicionais = dataDevolucao - dataPrevistaTermino;
            valorTotal += diasAdicionais.Days * 50; // R$50 por dia adicional
        }

        return valorTotal;
    }

    private decimal GetDiaryValue(int plano)
    {
        return plano switch
        {
            7 => 30m,
            15 => 28m,
            30 => 22m,
            45 => 20m,
            _ => 18m,
        };
    }

    private decimal GetPercentagePenalty(int plano)
    {
        return plano switch
        {
            7 => 0.20m,
            15 => 0.40m,
            _ => 0m,
        };
    }
}
