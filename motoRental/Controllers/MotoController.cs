using Microsoft.AspNetCore.Mvc;
using motoRental.Interfaces;
using motoRental.Models;
using motoRental.Services.ErrorHandking;

[ApiController]
[Route("motos")]
public class MotoController : Controller
{
    private readonly IMotoService _motoService;
    private readonly ILogger<MotoController> _logger;

    public MotoController(IMotoService motoService, ILogger<MotoController> logger)
    {
        _motoService = motoService;
        _logger = logger;
    }

    // POST: motos
    [HttpPost]
    public async Task<IActionResult> AddMoto([FromBody] Moto moto)
    {
        try
        {
            var createdMoto = await _motoService.AddMoto(moto);
            return CreatedAtAction(nameof(AddMoto), new { identificador = createdMoto.Identificador }, createdMoto);
        }
        catch (Exception ex)
        {
            return ErrorHandlingService.HandleError(ex);
        }
    }

    // GET: motos
    [HttpGet]
    public async Task<IActionResult> GetMotos([FromQuery] string placa = null)
    {
        try
        {
            var motos = await _motoService.GetMotos(placa);
            return Ok(motos);
        }
        catch (Exception ex)
        {
            return ErrorHandlingService.HandleError(ex);
        }
    }

    // GET: motos/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMotoById(string id)
    {
        try
        {
            var moto = await _motoService.GetMotoById(id);
            return Ok(moto);
        }
        catch (Exception ex)
        {
            return ErrorHandlingService.HandleError(ex);
        }
    }

    // PUT: motos/{id}/placa
    [HttpPut("{id}/placa")]
    public async Task<IActionResult> ChangeRegistration(string identificador, [FromBody] string novaPlaca)
    {
        try
        {
            await _motoService.ChangeRegistration(identificador, novaPlaca);
            return NoContent();
        }
        catch (Exception ex)
        {
            return ErrorHandlingService.HandleError(ex);
        }
    }

    // DELETE: motos/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMoto(string id)
    {
        try
        {
            await _motoService.DeleteMoto(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return ErrorHandlingService.HandleError(ex);
        }
    }
}
