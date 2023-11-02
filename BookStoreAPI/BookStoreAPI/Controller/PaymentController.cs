using BookStoreAPI.Core.DTO.Request;
using Microsoft.AspNetCore.Mvc;
using Service.Service.IService;

namespace BookStoreAPI.Controller;

[Route("api/payment")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IMoMoService _moMoService;

    public PaymentController(IMoMoService moMoService)
    {
        _moMoService = moMoService;
    }
    
    [HttpGet]
    [Route("momo-return")]
    public async Task<IActionResult> MomoReturn([FromQuery] MomoOneTimePaymentResultRequest request)
    {
        var response = await _moMoService.ProcessPaymentReturn(request);
        //return response.IsError ? HandleErrorResponse(response.Errors) : Redirect($"{response.Payload}/order-success");
        if (response)
        {
            return Redirect("http://localhost:3000/order-success");
        }
        return Redirect("http://localhost:3000/order-fail");
    }
}