using AutoMapper;
using BookStoreAPI.Core.DTO;
using BookStoreAPI.Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Service.Service;
using Service.Service.IService;
using static System.Net.Mime.MediaTypeNames;

namespace BookStoreAPI.Controller
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        IOrderService _order;
        IMapper _map;
        public OrderController(IOrderService order, IMapper mapper)
        {
            _map = mapper;
            _order = order;
        }
       /// <summary>
       /// Search order by order code or get all orders
       /// </summary>
       /// <param name="orderCode"></param>
       /// <returns></returns>
        [HttpGet()]
        public async Task<IActionResult> GetOrder(string? orderCode)
        {
            if (string.IsNullOrEmpty(orderCode))
            {
                var respone = await _order.GetAllOrder();
                if (respone != null)
                {
                    var result = _map.Map<IEnumerable<OrderDTO>>(respone);
                    return Ok(result);
                }
            }
            else
            {
                var respone = await _order.SearchByOrderCode(orderCode);
                if (respone != null)
                {
                    var result = _map.Map<OrderDTO>(respone);
                    return Ok(result);
                }
            }
            return BadRequest("order don't exists");
        }
        /// <summary>
        /// Get order by userId or orderId
        /// </summary>
        /// <param name="id"></param>
        /// <param name="option">1.ByUserId, 2.ByOrderId </param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderByUserId(Guid id, Enum.EnumClass.OrderOption option)
        {
            switch ((int)option)
            {
                case 1:
                    var result1 = await _order.GetOrderByUserId(id);
                    if (result1 != null)
                    {
                        var response = _map.Map<IEnumerable<OrderDTO>>(result1);
                        return Ok(response);
                    }
                    break;
                case 2:
                    var result2 = await _order.GetOrderByOrderId(id);
                    if (result2 != null)
                    {
                        var response = _map.Map<OrderDTO>(result2);
                        return Ok(response);
                    }
                    break;
            }
            return BadRequest("order don't exists");
        }
     
        [HttpGet("just-created")]
        public async Task<IActionResult> GetOrderId()
        {
            var result = await _order.GetOrderIdJustCreated();
            return Ok(result);
        }
        [HttpPost("")]
        public async Task<IActionResult> CreateOrder(OrderDTO dto)
        {
            if (dto != null)
            {
                var order=_map.Map<Order>(dto);
                var result = await _order.CreateOrder(order);
                if (!result.IsNullOrEmpty()) return Ok(result);
            }
            return BadRequest("Add Order Fail");
        }
        /// <summary>
        /// Update order status by orderId
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="option">1.Confirm, 2.Success, 3.Fail, 4.Delete, 5.Restore</param>
        /// <returns></returns>
        [HttpPatch("{orderId}")]
        public async Task<IActionResult> UpdateStatus(Guid orderId,Enum.EnumClass.OrderStatusOption option)
        {
            bool result = false;
            switch((int)option)
            {
                case 1:
                    result = await _order.ConfirmOrder(orderId);
                    if (result) return Ok("Update Confirm Status Successful");
                    break;
                case 2:
                    result = await _order.OrderSuccess(orderId);
                    if (result) return Ok("Update Success Status Successful");
                    break;
                case 3:
                    result = await _order.OrderFail(orderId);
                    if (result) return Ok("Update Fail Status Successful");
                    break;
                case 4:
                    result = await _order.DeleteOrder(orderId);
                    if (result) return Ok("Update Delete Status Successful");
                    break;
                case 5:
                    result = await _order.RestoreOrder(orderId);
                    if (result) return Ok("Update Restore Status Successful");
                    break;
            }
            return BadRequest("Update Status Failed");
        }
       
        [HttpDelete("")]
        public async Task<IActionResult> RemoveOrder(Guid orderId)
        {
            var result = await _order.RemoveOrder(orderId);
            if (result) return Ok("Remove Order Success");
            return BadRequest("Remove Order Fail");
        }
      
    }
}
