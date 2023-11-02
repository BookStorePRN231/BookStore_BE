using BookStoreAPI.Core.DTO.Request;
using BookStoreAPI.Core.Interface;
using BookStoreAPI.Core.Model;
using Microsoft.Extensions.Configuration;
using Service.Service.IService;

namespace Service.Service;

public class MoMoService : IMoMoService
{
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWorkRepository _unit;

    public MoMoService(IConfiguration configuration,IUnitOfWorkRepository unit)
    {
        _configuration = configuration;
        _unit = unit;
    }
    public async Task<(bool, string?)> CreateMomoPayment(Order order)
    {
        var momoOneTimePaymentRequest = new MomoOneTimePaymentRequest(_configuration["MomoSetting:PartnerCode"]!,
            DateTime.UtcNow.AddHours(7).Ticks.ToString() + order.Order_Code + "bookstore", (long)order.Order_Amount,
            order.Order_Id.ToString(),
            "Thanh toán BookStore", _configuration["MomoSetting:ReturnUrl"]!, _configuration["MomoSetting:IpnUrl"]!,
            "captureWallet", string.Empty);
        momoOneTimePaymentRequest.MakeSignature(_configuration["MomoSetting:AccessKey"]!,_configuration["MomoSetting:SecretKey"]!);
        return await momoOneTimePaymentRequest.GetLink(_configuration["MomoSetting:PaymentUrl"]!);
    }

    public async Task<bool> ProcessPaymentReturn(MomoOneTimePaymentResultRequest MomoOneTimePaymentResultRequest)
    {
        var isValidSignature = MomoOneTimePaymentResultRequest.IsValidSignature(_configuration["MomoSetting:AccessKey"]!, _configuration["MomoSetting:SecretKey"]!);
        if (isValidSignature)
        {
            // xử lý nghiêpj vụ gì đó
            var order = await _unit.Order.GetById(Guid.Parse(MomoOneTimePaymentResultRequest.orderId));
            if (order is not null)
            {
                return true;
            }
        }
        return false;
    }
}