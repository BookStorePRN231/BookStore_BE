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
    [Route("api/importation-detail")]
    [ApiController]
    public class ImportationDetailController : ControllerBase
    {
        IImportationDetailService _import;
        IMapper _map;
        public ImportationDetailController(IImportationDetailService import, IMapper mapper)
        {
            _import = import;
            _map = mapper;
        }
        /// <summary>
        /// Search book in importaion by name
        /// </summary>
        /// <param name="bookName"></param>
        /// <returns></returns>
        [HttpGet("search/{bookName}")]
        public async Task<IActionResult> SearchImportation(string bookName)
        {
            var respone = await _import.SearchImport(bookName);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest(bookName + " don't exists");
        }
        /// <summary>
        /// Get importation detail
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetImportationDetail()
        {
            var respone = await _import.GetDiplayImportDetail();
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest("importation detail don't exists");
        }
        /// <summary>
        /// Get Importation by importId
        /// </summary>
        /// <param name="importId"></param>
        /// <returns></returns>
        [HttpGet("{importId}")]
        public async Task<IActionResult> GetByImportId(Guid importId)
        {
            var respone = await _import.GetImportDetailByImportId(importId);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest("importation detail don't exists");
        }
        [HttpPost("")]
        public async Task<IActionResult> CreateImportationDetail(ImportationDetailDTO dto)
        {
            if (dto != null)
            {
                //update status request done
            if( await _import.UpdateStatusRequest(dto.Request_Id)) { 
                    var import = _map.Map<ImportationDetail>(dto);
                    var result = await _import.CreateImportDetail(import);
                    if (result) return Ok("Add Import Detail Success");
                }
               
            }
            return BadRequest("Add Import Detail Fail");
        }
    }
}
