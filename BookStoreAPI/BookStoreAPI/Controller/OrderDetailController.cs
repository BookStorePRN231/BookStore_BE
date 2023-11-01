using AutoMapper;
using BookStoreAPI.Core.DTO;
using BookStoreAPI.Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Service;
using Service.Service.IService;
using static System.Net.Mime.MediaTypeNames;

namespace BookStoreAPI.Controller
{
    [Route("api/order-detail")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        IOrderDetailService _order;
        IMapper _map;
        public OrderDetailController(IOrderDetailService order, IMapper mapper)
        {
            _map = mapper;
            _order = order;
        }
        /// <summary>
        /// Search by book name or get all order detail 
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public async Task<IActionResult> GetOrderDetail(string? bookName)
        {
            if (string.IsNullOrEmpty(bookName))
            {
                var respone = await _order.GetDisplayOrderDetail();
                if (respone != null)
                    return Ok(respone);
            }
            else
            {
                var respone = await _order.SearchOrder(bookName);
                if (respone != null)
                    return Ok(respone);
            }
            return BadRequest("order detail don't exists");
        }
        /// <summary>
        /// Gey order detail by order id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetByOrderId(Guid orderId)
        {
            var respone = await _order.GetOrderDetailByOrderId(orderId);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest("order detail don't exists");
        }
        [HttpPost()]
        public async Task<IActionResult> CreateOrderDetail(OrderDetailDTO dto)
        {
            if (dto != null)
            {
                var order = _map.Map<OrderDetail>(dto);
                var result = await _order.CreateOrderDetail(order);
                if (result) return Ok("Add Order Detail Success");
            }
            return BadRequest("Add Order Detail Fail");
        }
    }
}
