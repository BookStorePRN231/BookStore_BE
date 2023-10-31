using AutoMapper;
using BookStoreAPI.Core.DTO;
using BookStoreAPI.Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Service.IService;
using static System.Net.Mime.MediaTypeNames;

namespace BookStoreAPI.Controller
{
    [Route("api/inventories")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        IInventoryService _inventory;
        IMapper _map;
        public InventoryController(IInventoryService inventory,IMapper mapper)
        {
            _inventory = inventory;
            _map = mapper;
        }
        [HttpGet("")]
        public async Task<IActionResult> GetInventory()
        {
            var respone = await _inventory.GetAllInventory();
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest("inventory don't exists");
        }
        /// <summary>
        /// Search inventory by name book
        /// </summary>
        /// <param name="bookName"></param>
        /// <returns></returns>
        [HttpGet("{bookName}")]
        public async Task<IActionResult> SearchInventory(string bookName)
        {
            var respone = await _inventory.SearchInventory(bookName);
            if (respone != null)
            {
                return Ok(respone);
            }
            return BadRequest(bookName+" don't exists");
        }
        [HttpPost("")]
        public async Task<IActionResult> AddInventory(InventoryDTO dto)
        {
            if (dto != null)
            {
                var inventory=_map.Map<Inventory>(dto);
                var result = await _inventory.CreateInventory(inventory);
                if (result) return Ok("Add Inventory Success");
            }
            return BadRequest("Add Inventory Fail");
        }
        /// <summary>
        /// Delete or restore Inventory by inventoryId
        /// </summary>
        /// <param name="inventoryId"></param>
        /// <param name="option">1.Delete, 2.Restore</param>
        /// <returns></returns>
        [HttpPatch("{inventoryId}")]
        public async Task<IActionResult> DeleteBook(Guid inventoryId, Enum.EnumClass.CommonStatusOption option)
        {
            bool result = false;
            switch ((int)option)
            {
                case 1:
                    result = await _inventory.DeleteInventory(inventoryId);
                    if (result) return Ok("Delete Inventory Success");
                    break;
                case 2:
                    result = await _inventory.RestoreInventory(inventoryId);
                    if (result) return Ok("Restore Inventory Success");
                    break;
            }
            return BadRequest("Delete/Restore Inventory Failed");
        }
        [HttpDelete("")]
        public async Task<IActionResult> RemoveInventory(Guid inventoryId)
        {
            var result = await _inventory.RemoveInventory(inventoryId);
            if (result) return Ok("Remove Inventory Success");
            return BadRequest("Remove Inventory Fail");
        }
    }
}
