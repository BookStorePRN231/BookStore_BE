using AutoMapper;
using BookStoreAPI.Core.DTO;
using BookStoreAPI.Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.Service;
using Service.Service.IService;
using static System.Net.Mime.MediaTypeNames;

namespace BookStoreAPI.Controller
{
    [Route("api/importations")]
    [ApiController]
    public class ImportationController : ControllerBase
    {
        IImportationService _import;
        IMapper _map;
        public ImportationController(IImportationService import, IMapper mapper)
        {
            _import = import;
            _map = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetImportation()
        {
            var respone = await _import.GetDiplayImport();
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest("importation don't exists");
        }
        /// <summary>
        /// Get import id just created to create importation detail
        /// </summary>
        /// <returns></returns>
        [HttpGet("just-created")]
        public async Task<IActionResult> GetImportId()
        {
            var result = await _import.GetImportIdJustCreated();
            return Ok(result);
        }
        /// <summary>
        /// Export file excel by month
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        [HttpGet("export/{month}")]
        public async Task<IActionResult> ExportExcel(int month)
        {
            // Sử dụng từ khoá await ở đây để đợi cho đến khi phương thức ExporteExcel thực hiện xong.
            var response = await _import.ExporteExcel(month);
            return response; // Trả về phản hồi HTTP chứa tệp Excel.
        }
        [HttpPost]
        public async Task<IActionResult> CreateImportation(ImportationDTO dto)
        {
            if (dto != null)
            {
                var import=_map.Map<Importation>(dto);
                var result = await _import.CreateImport(import);
                if (result) return Ok("Add Import Success");
            }
            return BadRequest("Add Import Fail");
        }
        /// <summary>
        /// Delete or restore Importation by importId
        /// </summary>
        /// <param name="importId"></param>
        /// <param name="option">1.Delete, 2.Restore</param>
        /// <returns></returns>
        [HttpPatch("{importId}")]
        public async Task<IActionResult> DeleteBook(Guid importId, Enum.EnumClass.CommonStatusOption option)
        {
            bool result = false;
            switch ((int)option)
            {
                case 1:
                    result = await _import.DeleteImport(importId);
                    if (result) return Ok("Delete Importation Success");
                    break;
                case 2:
                    result = await _import.RestoreImport(importId);
                    if (result) return Ok("Restore Importation Success");
                    break;
            }
            return BadRequest("Delete/Restore Importation Failed");
        }
        [HttpDelete]
        public async Task<IActionResult> RemoveImportation(Guid importId)
        {
            var result = await _import.RemoveImport(importId);
            if (result) return Ok("Remove Importation Success");
            return BadRequest("Remove" +
                " Importation Fail");
        }
       
    }
}
