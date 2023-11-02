using AutoMapper;
using BookStoreAPI.Core.DTO;
using BookStoreAPI.Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Service.IService;
using static System.Net.Mime.MediaTypeNames;

namespace BookStoreAPI.Controller
{
    [Route("api/requests")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        //Cộng quantity khi import thành công, status order, order trừ quantity book

        IRequestService _request;
        IMapper _map;
        public RequestController(IRequestService request, IMapper mapper)
        {
            _request=request;
            _map = mapper;
        }
        
        [HttpGet()]
        public async Task<IActionResult> GetRequest()
        {
            var respone = await _request.GetAllRequest();
            if (respone.Count()>0)
            {
                return Ok(respone);
            }
            return BadRequest("request don't exists !");
        }
        [HttpGet("{requestId}")]
        public async Task<IActionResult> GetRequestById(Guid requestId)
        {
            var respone = await _request.GetRequestById(requestId);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest("request don't exists !");
        }
        /// <summary>
        /// Add old or new request
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="option">1.New, 2.Old</param>
        /// <returns></returns>
        [HttpPost()]
        public async Task<IActionResult> AddRequest(RequestDTO dto, Enum.EnumClass.CommonStatusOption option)
        {
            var request = _map.Map<BookingRequest>(dto);
            bool result = false;
            switch ((int)option)
            {
                case 1:
                    result = await _request.CreateRequest(request, true);
                    break;
                case 2:
                    result = await _request.CreateRequest(request, false);
                    break;
            }
            if (result) return Ok("Add Request Successful");
            return BadRequest("Add Request Failed");
        }
        [HttpPut()]
        public async Task<IActionResult> UpdateRequest(RequestDTO requestDTO)
        {
            if (requestDTO != null)
            {
                var request = _map.Map<BookingRequest>(requestDTO);
                var result = await _request.UpdateRequest(request);
                if (result) return Ok("Update Request Success");
            }
            return BadRequest("Update Request Fail");
        }
        /// <summary>
        /// Update undone status request by requestId
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        [HttpPatch("status/{requestId}")]
        public async Task<IActionResult> UnDoneRequest(Guid requestId, string note)
        {
            var result = await _request.UpdateStatusToUnDone(requestId, note);
            if (result) return Ok("UnDone Request Successful");
            return BadRequest("UnDone Request Failed");
        }
        /// <summary>
        /// Delete or restore Request by requestId
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="option">1.Delete, 2.Restore</param>
        /// <returns></returns>
        [HttpPatch("{requestId}")]
        public async Task<IActionResult> DeleteBook(Guid requestId, Enum.EnumClass.CommonStatusOption option)
        {
            bool result = false;
            switch ((int)option)
            {
                case 1:
                    result = await _request.DeleteRequest(requestId);
                    if (result) return Ok("Delete Request Successful");
                    break;
                case 2:
                    result = await _request.RestoreRequest(requestId);
                    if (result) return Ok("Restore Request Successful");
                    break;
            }
            return BadRequest("Delete/Restore Request Failed");
        }
        [HttpDelete()]
        public async Task<IActionResult> RemoveRequest(Guid requestId)
        {
            var result = await _request.RemoveRequest(requestId);
            if (result) return Ok("Remove Request Success");
            return BadRequest("Remove Request Fail");
        }
    }
}
