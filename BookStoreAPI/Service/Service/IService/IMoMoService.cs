using BookStoreAPI.Core.DTO.Request;
using BookStoreAPI.Core.Model;

namespace Service.Service.IService;

public interface IMoMoService
{
    public Task<(bool, string?)> CreateMomoPayment(Order order);
    public Task<bool> ProcessPaymentReturn(MomoOneTimePaymentResultRequest MomoOneTimePaymentResultRequest);
}