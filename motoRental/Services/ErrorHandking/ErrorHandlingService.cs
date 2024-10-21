using Microsoft.AspNetCore.Mvc;

namespace motoRental.Services.ErrorHandking
{
    public class ErrorHandlingService
    {
        public static IActionResult HandleError(Exception ex)
        {
            if (ex is InvalidOperationException)
            {
                //return new BadRequestObjectResult(new { message = ex.Message });
                return new BadRequestObjectResult(new { message = "Dados inválidos." });
            }
            else if (ex is KeyNotFoundException)
            {
                return new NotFoundObjectResult(new { message = ex.Message });
            }
            else
            {
                return new ObjectResult(new { message = "Erro interno do servidor.", details = ex.Message })
                {
                    StatusCode = 500
                };
            }
        }
    }
}
