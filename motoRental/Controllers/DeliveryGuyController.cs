using Microsoft.AspNetCore.Mvc;
using motoRental.Interfaces;
using motoRental.Models;
using motoRental.Services.ErrorHandking;

[Route("entregadores")]
[ApiController]
public class DeliveryGuyController : Controller
{
    private readonly IDeliveryGuyService _deliveryGuyService;
    private readonly ILogger<DeliveryGuyController> _logger;

    public DeliveryGuyController(IDeliveryGuyService deliveryGuyService, ILogger<DeliveryGuyController> logger)
    {
        _deliveryGuyService = deliveryGuyService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> AddDeliveryGuy([FromBody] DeliveryGuy entregador)
    {
        try
        {
            var createdDeliveryGuy = await _deliveryGuyService.AddDeliveryGuy(entregador);
            return CreatedAtAction(nameof(AddDeliveryGuy), new { identificador = createdDeliveryGuy.Identificador }, createdDeliveryGuy);
        }
        catch (Exception ex)
        {
            return ErrorHandlingService.HandleError(ex);
        }
    }

    [HttpPut("{id}/cnh")]
    public async Task<IActionResult> UpdatePhotoCnh(string identificador, [FromBody] string imagemCnh)
    {
        try
        {
            await _deliveryGuyService.UpdatePhotoCnh(identificador, imagemCnh);
            return NoContent();
        }
        catch (Exception ex)
        {
            return ErrorHandlingService.HandleError(ex);
        }
    }
}
