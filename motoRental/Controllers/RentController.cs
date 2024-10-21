using Microsoft.AspNetCore.Mvc;
using motoRental.DTO;
using motoRental.Models;
using motoRental.Services.ErrorHandking;

namespace motoRental.Controllers
{
    [ApiController]
    [Route("locacao")]
    public class RentController : Controller
    {
        private readonly RentService _rentService;

        public RentController(RentService rentService)
        {
            _rentService = rentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewRent([FromBody] RentRequestDTO rentRequest)
        {
            try
            {
                var novaLocacao = await _rentService.CreateNewRentAsync(rentRequest);
                return CreatedAtAction(nameof(ConsultRentById), new { id = novaLocacao.Identificador }, novaLocacao);
            }
            catch (Exception ex)
            {
                return ErrorHandlingService.HandleError(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Rent>> ConsultRentById(string id)
        {
                var locacao = await _rentService.GetRentByIdAsync(id);
                return Ok(locacao);
        }

        [HttpPut("{id}/devolucao")]
        public async Task<IActionResult> ReturnMoto(string id, [FromBody] DateTime dataDevolucao)
        {
            try
            {
                var locacao = await _rentService.GetRentByIdAsync(id);
                locacao.Data_Termino = dataDevolucao;

                var valorTotal = _rentService.CalculateRentValue(locacao, locacao.Data_Prevista_Termino, locacao.Data_Termino.Value);

                return Ok(new
                {
                    Identificador = locacao.Identificador,
                    ValorTotal = valorTotal,
                    DataDevolucao = locacao.Data_Termino
                });
            }
            catch (Exception ex)
            {
                return ErrorHandlingService.HandleError(ex);
            }
        }
    }
}
